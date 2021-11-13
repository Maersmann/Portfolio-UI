using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.AuswahlMessages;
using Data.Model.AuswertungModels;
using Data.Types.AuswertungTypes;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
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
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace Logic.UI.AuswertungViewModels
{
    public class DividendeWertpapierEntwicklungMonatlichViewModel : ViewModelAuswertung<DividendeWertpapierEntwicklungMonatlichModel>
    {
        private int jahrvon;
        private int jahrbis;
        private int wertpapierID;
        private bool bruttoSeriesVisibility;
        private bool nettoSeriesVisibility;
        public DividendeWertpapierEntwicklungMonatlichViewModel()
        {
            nettoSeriesVisibility = true;
            bruttoSeriesVisibility = true;
            Data = new DividendeWertpapierEntwicklungMonatlichModel();
            Title = "Auswertung Entwicklung Dividende Wertpapier";
            SecondTitle = "";
            jahrvon = DateTime.Now.Year;
            jahrbis = DateTime.Now.Year;
            wertpapierID = 0;
            AuswahlCommand = new RelayCommand(() => ExcecuteAuswahlCommand());
            Formatter = value => string.Format("{0:N2}€", value);
        }

        private void ExcecuteAuswahlCommand()
        {
            Messenger.Default.Send(new OpenWertpapierAuswahlMessage(OpenOpenWertpapierAuswahlMessageCallback), "DividendeWertpapierEntwicklung");
        }

        private async void LoadData()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividendenErhalten/Gesamt/Wertpapiere/{wertpapierID}/Entwicklung?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                Data = await resp.Content.ReadAsAsync<DividendeWertpapierEntwicklungMonatlichModel>();

                ChartValues<double> NettoChart = new ChartValues<double>();
                ChartValues<double> BruttoChart = new ChartValues<double>();
                Labels = new string[Data.Betraege.Count];
                int index = 0;

                Data.Betraege.ToList().ForEach(a =>
                {
                    NettoChart.Add(a.Netto);
                    BruttoChart.Add(a.Brutto);
                    Labels[index] = a.Datum.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
                    index++;

                    if (a.Brutto > HighestValue)
                    {
                        HighestValue = a.Brutto;
                    }
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

                var NettoSeries = new LineSeries
                {
                    Values = NettoChart,
                    Title = "Netto",
                };
                var BruttoSeries = new LineSeries
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

                RaisePropertyChanged(nameof(Data));
                RaisePropertyChanged(nameof(SeriesCollection));
                RaisePropertyChanged(nameof(Labels));
                RaisePropertyChanged(nameof(Formatter)); 
            }
            RequestIsWorking = false;
        }
        #region Callbacks
        private void OpenOpenWertpapierAuswahlMessageCallback(bool confirmed, int id)
        {
            if (confirmed)
            {
                wertpapierID = id;
                LoadData();
            }
        }
        #endregion

        #region Bindings
        public int? JahrVon
        {
            get => jahrvon;
            set
            {
                ValidatZahl(value, nameof(JahrVon));
                this.RaisePropertyChanged();
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
                jahrbis = value.GetValueOrDefault(0);
            }
        }
        public ICommand AuswahlCommand { get; set; }
        public string SecondTitle { get; set; }
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
