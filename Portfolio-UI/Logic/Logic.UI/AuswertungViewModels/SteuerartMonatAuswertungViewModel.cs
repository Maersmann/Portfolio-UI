using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Data.Model.AuswertungModels;
using LiveCharts;
using LiveCharts.Definitions.Series;
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
    public class SteuerartMonatAuswertungViewModel : ViewModelAuswertung<SteuerartMonatAuswertungModel>
    {
        private int jahrvon;
        private int jahrbis;
        public SteuerartMonatAuswertungViewModel()
        {
            Title = "Auswertung Steuerart je Monat";
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
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/steuern/Steuerart/Monate?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<SteuerartMonatAuswertungModel>>();

                Labels = new string[ItemList.Count];
                int index = 0;
                SeriesCollection = new SeriesCollection();
                ItemList.ToList().ForEach(item =>
                {
                    item.Steuerarten.ToList().ForEach(steuer =>{
                        ISeriesView StackedColoumn = SeriesCollection.ToList().Find(series => series.Title.Equals(steuer.Steuerart));
                        if (StackedColoumn == null)
                        {
                            StackedColoumn = new StackedColumnSeries
                            {
                                Values = new ChartValues<double>(),
                                Title = steuer.Steuerart
                            };
                            SeriesCollection.Add(StackedColoumn);
                        }

                        StackedColoumn.Values.Add(steuer.Betrag);
                    });

                    Labels[index] = item.Datum.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
                    
                    index++;
                });

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
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateAnzahl(zahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }
        #endregion
    }
}
