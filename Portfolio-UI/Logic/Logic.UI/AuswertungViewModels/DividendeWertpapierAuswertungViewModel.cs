using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.AuswertungModels;
using LiveCharts;
using LiveCharts.Wpf;
using Logic.UI.BaseViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace Logic.UI.AuswertungViewModels
{
    public class DividendeWertpapierAuswertungViewModel : ViewModelAuswertung<DividendeWertpapierAuswertungModel>
    {
        private int jahrvon;
        private int jahrbis;

        public DividendeWertpapierAuswertungViewModel()
        {
            Title = "Auswertung Dividende je Wertpapier";
            jahrvon = DateTime.Now.Year;
            jahrbis = DateTime.Now.Year;
            Formatter = value => value.ToString("0.## €");
            LoadDataCommand = new DelegateCommand(this.ExcecuteLoadDataCommand, this.CanExcecuteLoadDataCommand);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        public async void ExcecuteLoadDataCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividenden/Wertpapiere?jahrVon={jahrvon}&jahrBis={jahrbis}");
                if (resp.IsSuccessStatusCode)
                    ItemList = await resp.Content.ReadAsAsync<ObservableCollection<DividendeWertpapierAuswertungModel>>();

                ChartValues<double> values = new ChartValues<double>();

                static string labelPoint(ChartPoint chartPoint) =>
                    string.Format("{0:N2}€ ({1:P})", chartPoint.Y, chartPoint.Participation);

                SeriesCollection = new SeriesCollection();

                ItemList.ToList().ForEach(a =>
                {
                    SeriesCollection.Add(new PieSeries { Values = new ChartValues<double> { a.Betrag } , Title = a.Bezeichnung, DataLabels = true, LabelPoint = labelPoint });
                });
               

                RaisePropertyChanged(nameof(SeriesCollection));
                RaisePropertyChanged(nameof(Formatter));
            }
            RaisePropertyChanged("ItemList");
        }

        #region Bindings
        public ICommand LoadDataCommand { get; set; }
        public int? JahrVon
        {
            get => jahrvon;
            set
            {
                ValidatZahl(value, nameof(JahrVon));
                this.RaisePropertyChanged();
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
                this.RaisePropertyChanged();
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
