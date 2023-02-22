using Aktien.Logic.Core.Validierung.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.AuswertungModels.DividendeModels;
using Data.Types.AuswertungTypes;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Data.Model.AuswertungModels.ZinsenModels;
using System.Linq;

namespace Logic.UI.AuswertungViewModels.ZinsenViewModels
{
    public class ZinsenMonatlichJahresentwicklungViewModel : ViewModelAuswertung<ZinsenMonatlichJahresentwicklungModel>
    {
        private int jahrvon;
        private int jahrbis;
        private ZinsenBetragTyp typ;

        public ZinsenMonatlichJahresentwicklungViewModel()
        {
            Title = "Auswertung Zinsen je Monat - Jahresentwicklung";
            jahrvon = GlobalUserVariables.JahrVon;
            jahrbis = DateTime.Now.Year;
            typ = ZinsenBetragTyp.Erhalten;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/zinsen/Monatlich/Jahresentwicklung?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<ZinsenMonatlichJahresentwicklungModel>>();
                SetDataIntoChart();
            }
            RequestIsWorking = false;
        }

        private void SetDataIntoChart()
        {
            Labels = new string[12];
            LineSeries<decimal>[] series = new LineSeries<decimal>[ItemList.Count];
            int index = 0;
            ItemList.ToList().ForEach(item =>
            {
                LineSeries<decimal> coloumn = new LineSeries<decimal>
                {
                    Name = item.Jahr.ToString(),
                    Values = new List<decimal>(),
                    TooltipLabelFormatter = (point) => point.Label + point.PrimaryValue.ToString("N2") + "€",
                };
                coloumn.TooltipLabelFormatter = (point) => item.Jahr + " " + point.PrimaryValue.ToString("N2") + "€";
                List<decimal> betraege = new List<decimal>();

                item.Werte.ToList().ForEach(mw =>
                {
                    decimal Betrag = typ.Equals(ZinsenBetragTyp.Erhalten) ? mw.Erhalten : mw.Gesamt;
                    betraege.Add(Betrag);
                });
                coloumn.Values = betraege;
                series.SetValue(coloumn, index);
                index++;
            });

            for (int monat = 1; monat <= 12; monat++)
            {
                Labels[monat - 1] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monat);
            }

            XAxes.First().Labels = Labels;
            XAxes.First().Name = "Monat";
            YAxes.First().Name = "Betrag";
            Series = series;

            RaisePropertyChanged(nameof(Series));
            RaisePropertyChanged(nameof(Labels));
            RaisePropertyChanged(nameof(XAxes));
            RaisePropertyChanged(nameof(YAxes));
        }

        #region Bindings
        public ICommand LoadDataCommand { get; set; }
        public int? JahrVon
        {
            get => jahrvon;
            set
            {
                ValidatZahl(value, nameof(JahrVon));
                RaisePropertyChanged();
                ((DelegateCommand)LoadDataCommand).RaiseCanExecuteChanged();
                jahrvon = value.GetValueOrDefault(0);
            }
        }
        public int? JahrBis
        {
            get => jahrbis;
            set
            {
                ValidatZahl(value, nameof(JahrBis));
                RaisePropertyChanged();
                ((DelegateCommand)LoadDataCommand).RaiseCanExecuteChanged();
                jahrbis = value.GetValueOrDefault(0);
            }
        }

        public static IEnumerable<ZinsenBetragTyp> Types => Enum.GetValues(typeof(ZinsenBetragTyp)).Cast<ZinsenBetragTyp>();
        public ZinsenBetragTyp Typ
        {
            get => typ;
            set
            {
                RaisePropertyChanged();
                typ = value;
                SetDataIntoChart();
            }
        }
        #endregion

        #region Validate
        private bool ValidatZahl(int? zahl, string fieldname)
        {
            BaseValidierung Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateAnzahl(zahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }
        #endregion
    }
}
