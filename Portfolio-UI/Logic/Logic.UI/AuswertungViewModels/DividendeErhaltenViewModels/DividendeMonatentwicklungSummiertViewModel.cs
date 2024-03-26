using Aktien.Logic.Core.Validierung.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.AuswertungModels.DividendeModels;
using LiveChartsCore.SkiaSharpView;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.AuswertungViewModels.DividendeErhaltenViewModels
{
    public class DividendeMonatentwicklungSummiertViewModel : ViewModelAuswertung<DividendeMonatentwicklungSummiertModel>
    {
        private int jahrvon;
        private int jahrbis;
        private int monat;
        private LineSeries<double> nettoSeries;
        private LineSeries<double> bruttoSeries;
        public DividendeMonatentwicklungSummiertViewModel()
        {
            Title = "Auswertung Dividende Summiert im Monat";
            jahrvon = GlobalUserVariables.JahrVon;
            jahrbis = DateTime.Now.Year;
            monat = DateTime.Now.Month;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
            nettoSeries = new LineSeries<double>();
            bruttoSeries = new LineSeries<double>();
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividendenErhalten/Summiert/Monat?monat={monat}&jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<DividendeMonatentwicklungSummiertModel>>();

                IList<double> NettoChart = [];
                IList<double> BruttoChart = [];
                Labels = new string[ItemList.Count];
                int index = 0;

                ItemList.ToList().ForEach(a =>
                {
                    NettoChart.Add(a.Netto);
                    BruttoChart.Add(a.Brutto);
                    Labels[index] = a.Jahr.ToString();
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
        public int? Monat
        {
            get => monat;
            set
            {
                ValidatZahl(value, nameof(Monat));
                OnPropertyChanged();
                ((DelegateCommand)LoadDataCommand).RaiseCanExecuteChanged();
                monat = value.GetValueOrDefault(0);
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
