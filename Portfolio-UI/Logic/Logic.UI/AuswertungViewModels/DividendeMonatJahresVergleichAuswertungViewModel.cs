using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Data.Model.AuswertungModels;
using LiveCharts;
using LiveCharts.Wpf;
using Logic.UI.BaseViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.AuswertungViewModels
{
    public class DividendeMonatJahresVergleichAuswertungViewModel : ViewModelAuswertung<DividendeMonatJahresVergleichAuswertungModel>
    {
        private int jahrvon;
        private int jahrbis;
        public DividendeMonatJahresVergleichAuswertungViewModel()
        {
            Title = "Auswertung Dividende je Monat - Jahresvergleich";
            jahrvon = DateTime.Now.Year;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(this.ExcecuteLoadDataCommand, this.CanExcecuteLoadDataCommand);
            Formatter = value => value.ToString("0.## €");
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividenden/Monate/Jahresvergleich?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<DividendeMonatJahresVergleichAuswertungModel>>();

                ChartValues<double> values = new ChartValues<double>();
                Labels = new string[12];
                int index = 0;
                SeriesCollection = new SeriesCollection();
                ItemList.ToList().ForEach(item =>
                {
                    ColumnSeries coloumn = new ColumnSeries
                    {
                        Title = item.Jahr.ToString(),
                        Values = new ChartValues<double>()
                    };
                    item.Monatswerte.ToList().ForEach(jw =>
                    {
                        coloumn.Values.Add(jw.Betrag);
                    });
                    SeriesCollection.Add(coloumn);

                    
                    index++;
                });

                for (int monat = 1; monat <= 12; monat++)
                {
                    Labels[monat - 1] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monat);
                }

                RaisePropertyChanged(nameof(SeriesCollection));
                RaisePropertyChanged(nameof(Labels));
                RaisePropertyChanged(nameof(Formatter));
            }
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
            BaseValidierung Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateAnzahl(zahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }
        #endregion
    }
}
