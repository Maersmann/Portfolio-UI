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
        private bool sonderdividendeEinbeziehen;


        public DividendeVergleichMonatViewModel()
        {
            Title = "Auswertung Dividende je Monat - Jahresvergleich";
            jahrvon = GlobalUserVariables.JahrVon;
            jahrbis = DateTime.Now.Year;
            typ = DividendenBetragTyp.Netto;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
            SonderdividendeEinbeziehen = false;
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividendenErhalten/Gesamt/Monat/Vergleich?jahrVon={jahrvon}&jahrBis={jahrbis}&sonderdividendeEinbeziehen={sonderdividendeEinbeziehen}");
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
                ColumnSeries<double> coloumn = new()
                {
                    Name = item.Jahr.ToString(),
                    Values = [],
                };
                List<double> betraege = [];
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

        public static IEnumerable<DividendenBetragTyp> Types => Enum.GetValues(typeof(DividendenBetragTyp)).Cast<DividendenBetragTyp>();
        public DividendenBetragTyp Typ
        {
            get => typ;
            set
            {
                OnPropertyChanged();
                typ = value;
                SetDataIntoChart();
            }
        }

        public bool SonderdividendeEinbeziehen
        {
            get { return sonderdividendeEinbeziehen; }
            set
            {
                sonderdividendeEinbeziehen = value;
                OnPropertyChanged();
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
