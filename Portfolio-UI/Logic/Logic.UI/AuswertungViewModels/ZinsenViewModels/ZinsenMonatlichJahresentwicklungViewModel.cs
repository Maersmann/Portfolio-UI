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
                LineSeries<decimal> coloumn = new()
                {
                    Name = item.Jahr.ToString(),
                    Values = [],
                };
                List<decimal> betraege = [];

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

            OnPropertyChanged(nameof(Series));
            OnPropertyChanged(nameof(Labels));
            OnPropertyChanged(nameof(XAxes));
            OnPropertyChanged(nameof(YAxes));
        }

        #region Bindings
        public ICommand LoadDataCommand { get; set; }
        public int? JahrVon
        {
            get => jahrvon;
            set
            {
                ValidatZahl(value, nameof(JahrVon));
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
                typ = value;
                SetDataIntoChart();
            }
        }
        #endregion

        #region Validate
        private bool ValidatZahl(int? zahl, string fieldname)
        {
            BaseValidierung Validierung = new();

            bool isValid = Validierung.ValidateAnzahl(zahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }
        #endregion
    }
}
