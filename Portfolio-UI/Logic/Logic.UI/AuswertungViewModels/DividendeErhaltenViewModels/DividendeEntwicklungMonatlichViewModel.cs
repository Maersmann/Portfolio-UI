using Aktien.Logic.Core.Validierung.Base;
using Data.Model.AuswertungModels;
using LiveCharts;
using LiveCharts.Wpf;
using Base.Logic.ViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using Base.Logic.Core;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;

namespace Logic.UI.AuswertungViewModels
{
    public class DividendeEntwicklungMonatlichViewModel : ViewModelAuswertung<DividendeEntwicklungMonatlichModel>
    {
        private int jahrvon;
        private int jahrbis;
        private bool bruttoSeriesVisibility;
        private bool nettoSeriesVisibility;
        public DividendeEntwicklungMonatlichViewModel()
        {
            nettoSeriesVisibility = true;
            bruttoSeriesVisibility = true;
            Title = "Auswertung Dividende je Monat";
            jahrvon = DateTime.Now.Year;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
            Formatter = value => string.Format("{0:N2}€", value);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividendenErhalten/Gesamt/Monat/Entwicklung?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<DividendeEntwicklungMonatlichModel>>();

                ChartValues<double> NettoChart = new ChartValues<double>();
                ChartValues<double> BruttoChart = new ChartValues<double>();
                Labels = new string[ItemList.Count];
                int index = 0;
                
                ItemList.ToList().ForEach(a =>
                {
                    NettoChart.Add(a.Netto);
                    BruttoChart.Add(a.Brutto);
                    if(a.Brutto > HighestValue)
                    {
                        HighestValue = a.Brutto;
                    }
                    Labels[index] = a.Datum.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
                    index++;
                });

                Binding NettoSeriesVisbilityBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(nameof(NettoSeriesVisibility)),
                    Converter = new BooleanToVisibilityConverter(),
                    Mode = BindingMode.OneWay,
                };
                Binding BruttoSeriesVisbilityBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(nameof(BruttoSeriesVisibility)),
                    Converter = new BooleanToVisibilityConverter(),
                    Mode = BindingMode.OneWay,
                };

                var NettoSeries = new ColumnSeries
                {
                    Values = NettoChart,
                    Title = "Netto",
                };
                var BruttoSeries = new ColumnSeries
                {
                    Values = BruttoChart,
                    Title = "Brutto"
                };

                NettoSeries.SetBinding(UIElement.VisibilityProperty, NettoSeriesVisbilityBinding);
                BruttoSeries.SetBinding(UIElement.VisibilityProperty, BruttoSeriesVisbilityBinding);

                SeriesCollection = new SeriesCollection
                {
                    NettoSeries,
                    BruttoSeries
                };

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

        public bool BruttoSeriesVisibility
        {
            get { return bruttoSeriesVisibility; }
            set
            {
                bruttoSeriesVisibility = value;
                RaisePropertyChanged();
            }
        }

        public bool NettoSeriesVisibility
        {
            get { return nettoSeriesVisibility; }
            set
            {
                nettoSeriesVisibility = value;
                RaisePropertyChanged();
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
