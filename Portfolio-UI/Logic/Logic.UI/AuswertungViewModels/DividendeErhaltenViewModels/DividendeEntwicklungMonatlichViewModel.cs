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
        private ColumnSeries<double> nettoSeries;
        private ColumnSeries<double> bruttoSeries;
        public DividendeEntwicklungMonatlichViewModel()
        {
            Title = "Auswertung Dividende je Monat";
            jahrvon = DateTime.Now.Year;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);     
            nettoSeries = new ColumnSeries<double>();
            bruttoSeries = new ColumnSeries<double>();
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

                IList<double> NettoChart = new List<double>();
                IList<double> BruttoChart = new List<double>();
                Labels = new string[ItemList.Count];
                int index = 0;
                
                ItemList.ToList().ForEach(a =>
                {
                    NettoChart.Add(a.Netto);
                    BruttoChart.Add(a.Brutto);
                    Labels[index] = a.Datum.ToString("MM.yyyy", CultureInfo.CurrentCulture);
                    index++;
                });

                nettoSeries = new ColumnSeries<double>
                {
                    Values = NettoChart,
                    Name = "Netto",
                    TooltipLabelFormatter = (point) => "Netto " + point.PrimaryValue.ToString("N2") + "€"
                };
                bruttoSeries = new ColumnSeries<double>
                {
                    Values = BruttoChart,
                    Name = "Brutto",
                    TooltipLabelFormatter = (point) => "Brutto " + point.PrimaryValue.ToString("N2") + "€"
                };

                XAxes.First().Labels = Labels;
                XAxes.First().Name = "Monat";
                YAxes.First().Name = "Betrag";
                Series = new ColumnSeries<double>[2] { bruttoSeries, nettoSeries };

                RaisePropertyChanged(nameof(Series));
                RaisePropertyChanged(nameof(XAxes));
                RaisePropertyChanged(nameof(YAxes));
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
            get { return bruttoSeries.IsVisible; }
            set
            {
                bruttoSeries.IsVisible = value;
                RaisePropertyChanged(nameof(Series));
            }
        }
        public bool NettoSeriesVisibility
        {
            get { return nettoSeries.IsVisible; }
            set
            {
                nettoSeries.IsVisible = value;
                RaisePropertyChanged(nameof(Series));
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
