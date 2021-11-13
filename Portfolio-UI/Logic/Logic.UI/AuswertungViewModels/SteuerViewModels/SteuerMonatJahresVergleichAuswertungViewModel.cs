using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Data.Model.AuswertungModels;
using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
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

namespace Logic.UI.AuswertungViewModels
{
    public class SteuerMonatJahresVergleichAuswertungViewModel : ViewModelAuswertung<SteuerMonatJahresVergleichAuswertungModel>
    {
        private int jahrvon;
        private int jahrbis;
        public SteuerMonatJahresVergleichAuswertungViewModel()
        {
            Title = "Auswertung Steuerart je Monat - Jahresvergleich";
            jahrvon = DateTime.Now.Year;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(this.ExcecuteLoadDataCommand, this.CanExcecuteLoadDataCommand);
            Formatter = value => string.Format("{0:N2}€", value);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/steuern/Monate/Jahresvergleich?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<SteuerMonatJahresVergleichAuswertungModel>>();

                Labels = new string[12];
                SeriesCollection = new SeriesCollection();
                ItemList.ToList().ForEach(item =>
                {
                    ColumnSeries coloumn = new ColumnSeries
                    {
                        Title = item.Jahr.ToString(),
                        Values = new ChartValues<double>()
                    };
                    item.Monatswerte.ToList().ForEach(mw =>
                    {
                        coloumn.Values.Add(mw.Betrag);
                        if (Math.Abs(mw.Betrag) > HighestValue)
                        {
                            HighestValue = Math.Abs(mw.Betrag);
                        }
                    });
                    SeriesCollection.Add(coloumn);

                });

                for (int monat = 1; monat <= 12; monat++)
                {
                    Labels[monat - 1] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monat);
                }

                BerechneSeperator();

                RaisePropertyChanged(nameof(SeriesCollection));
                RaisePropertyChanged(nameof(Labels));
                RaisePropertyChanged(nameof(Formatter));
            }
            RequestIsWorking = false;
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
        #endregion

        #region Validate
        private bool ValidatZahl(int? zahl, string fieldname)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateAnzahl(zahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }
        #endregion
    }
}
