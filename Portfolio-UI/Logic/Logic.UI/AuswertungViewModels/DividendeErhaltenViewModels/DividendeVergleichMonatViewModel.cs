using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Data.Model.AuswertungModels;
using Base.Logic.ViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Base.Logic.Core;
using Data.Types.AuswertungTypes;
using LiveChartsCore.SkiaSharpView;

namespace Logic.UI.AuswertungViewModels
{
    public class DividendeVergleichMonatViewModel : ViewModelAuswertung<DividendeVergleichMonatModel>
    {
        private int jahrvon;
        private int jahrbis;
        private DividendenBetragTyp typ;

        public DividendeVergleichMonatViewModel()
        {
            Title = "Auswertung Dividende je Monat - Jahresvergleich";
            jahrvon = GlobalUserVariables.JahrVon;
            jahrbis = DateTime.Now.Year;
            typ = DividendenBetragTyp.Netto;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividendenErhalten/Gesamt/Monat/Vergleich?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<DividendeVergleichMonatModel>>();
                SetDataIntoChart();


            }
            RequestIsWorking = false;
        }

        private void SetDataIntoChart()
        {
            Labels = new string[12];
            ColumnSeries<double>[] series = new ColumnSeries<double>[ItemList.Count];
            int index = 0;
            ItemList.ToList().ForEach(item =>
            {
                ColumnSeries<double> coloumn = new ColumnSeries<double>
                {
                    Name = item.Jahr.ToString(),
                    Values = new List<double>(),
                    TooltipLabelFormatter = (point) => point.Label + point.PrimaryValue.ToString("N2") + "€",
                };
                coloumn.TooltipLabelFormatter = (point) => item.Jahr + " " + point.PrimaryValue.ToString("N2") + "€";
                List<double> betraege = new List<double>();
                item.Monatswerte.ToList().ForEach(mw =>
                {
                    double Betrag = typ.Equals(DividendenBetragTyp.Brutto) ? mw.Brutto : mw.Netto;
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

        public static IEnumerable<DividendenBetragTyp> Types => Enum.GetValues(typeof(DividendenBetragTyp)).Cast<DividendenBetragTyp>();
        public DividendenBetragTyp Typ
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
