using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.AuswahlMessages;
using Data.Model.AuswertungModels;
using Data.Types.AuswertungTypes;
using CommunityToolkit.Mvvm.Messaging;
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
using LiveChartsCore.SkiaSharpView;
using CommunityToolkit.Mvvm.Input;


namespace Logic.UI.AuswertungViewModels
{
    public class DividendeWertpapierEntwicklungMonatlichViewModel : ViewModelAuswertung<DividendeWertpapierEntwicklungMonatlichModel>
    {
        private int jahrvon;
        private int jahrbis;
        private int wertpapierID;
        private LineSeries<double> nettoSeries;
        private LineSeries<double> bruttoSeries;
        public DividendeWertpapierEntwicklungMonatlichViewModel()
        {
            nettoSeries = new LineSeries<double>();
            bruttoSeries = new LineSeries<double>();
            Data = new DividendeWertpapierEntwicklungMonatlichModel();
            Title = "Auswertung Entwicklung Dividende Wertpapier";
            SecondTitle = "";
            jahrvon = GlobalUserVariables.JahrVon;
            jahrbis = DateTime.Now.Year;
            wertpapierID = 0;
            AuswahlCommand = new RelayCommand(() => ExcecuteAuswahlCommand());
        }

        private void ExcecuteAuswahlCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenWertpapierAuswahlMessage(OpenWertpapierAuswahlMessageCallback), "DividendeWertpapierEntwicklung");
        }

        private async void LoadData()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividendenErhalten/Gesamt/Wertpapiere/{wertpapierID}/Entwicklung?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                Data = await resp.Content.ReadAsAsync<DividendeWertpapierEntwicklungMonatlichModel>();

                IList<double> NettoChart = [];
                IList<double> BruttoChart = [];
                Labels = new string[Data.Betraege.Count];
                int index = 0;

                Data.Betraege.ToList().ForEach(a =>
                {
                    NettoChart.Add(a.Netto);
                    BruttoChart.Add(a.Brutto);
                    Labels[index] = a.Datum.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
                    index++;
                });

                nettoSeries = new LineSeries<double>
                {
                    Values = NettoChart,
                    Name = "Netto",
                };
                bruttoSeries = new LineSeries<double>
                {
                    Values = BruttoChart,
                    Name = "Brutto",
                };

                XAxes.First().Labels = Labels;
                XAxes.First().Name = "Monat";
                YAxes.First().Name = "Betrag";
                Series = new LineSeries<double>[2] { bruttoSeries, nettoSeries };

                OnPropertyChanged(nameof(Series));
                OnPropertyChanged(nameof(XAxes));
                OnPropertyChanged(nameof(YAxes));
                OnPropertyChanged(nameof(Data));
            }
            RequestIsWorking = false;
        }
        #region Callbacks
        private void OpenWertpapierAuswahlMessageCallback(bool confirmed, int id)
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
                this.OnPropertyChanged();
                jahrvon = value.GetValueOrDefault(0);
            }
        }
        public int? JahrBis
        {
            get => jahrbis;
            set
            {
                ValidatZahl(value, nameof(JahrBis));
                this.OnPropertyChanged();
                jahrbis = value.GetValueOrDefault(0);
            }
        }
        public ICommand AuswahlCommand { get; set; }
        public string SecondTitle { get; set; }
        public bool BruttoSeriesVisibility
        {
            get { return bruttoSeries.IsVisible; }
            set
            {
                bruttoSeries.IsVisible = value;
                OnPropertyChanged(nameof(Series));
            }
        }

        public bool NettoSeriesVisibility
        {
            get { return nettoSeries.IsVisible; }
            set
            {
                nettoSeries.IsVisible = value;
                OnPropertyChanged(nameof(Series));
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
