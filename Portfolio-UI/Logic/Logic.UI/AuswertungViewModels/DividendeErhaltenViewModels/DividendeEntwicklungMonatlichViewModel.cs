using Aktien.Logic.Core.Validierung.Base;
using Data.Model.AuswertungModels;

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
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Measure;
using SkiaSharp;

namespace Logic.UI.AuswertungViewModels
{
    public class DividendeEntwicklungMonatlichViewModel : ViewModelAuswertung<DividendeEntwicklungMonatlichModel>
    {
        private int jahrvon;
        private int jahrbis;
        private LineSeries<double> nettoSeries;
        private LineSeries<double> bruttoSeries;
        private bool sonderdividendeEinbeziehen;
        public DividendeEntwicklungMonatlichViewModel()
        {
            Title = "Auswertung Dividende je Monat";
            jahrvon = GlobalUserVariables.JahrVon;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);     
            nettoSeries = new LineSeries<double>();
            bruttoSeries = new LineSeries<double>();
            SonderdividendeEinbeziehen = false;
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividendenErhalten/Gesamt/Monat/Entwicklung?jahrVon={jahrvon}&jahrBis={jahrbis}&sonderdividendeEinbeziehen={sonderdividendeEinbeziehen}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<DividendeEntwicklungMonatlichModel>>();

                IList<double> NettoChart = [];
                IList<double> BruttoChart = [];
                Labels = new string[ItemList.Count];
                int index = 0;
                
                ItemList.ToList().ForEach(a =>
                {
                    NettoChart.Add(a.Netto);
                    BruttoChart.Add(a.Brutto);
                    Labels[index] = a.Datum.ToString("MM.yyyy", CultureInfo.CurrentCulture);
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
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateAnzahl(zahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }
        #endregion

    }
}
