using Aktien.Logic.Core.Validierung.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.AuswertungModels.DividendeModels;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Data.Model.AuswertungModels.ZinsenModels;
using System.Linq;
using System.Globalization;

namespace Logic.UI.AuswertungViewModels.ZinsenViewModels
{
    public class ZinsenGesamtentwicklungMonatlichSummiertViewModel : ViewModelAuswertung<ZinsenGesamtentwicklungMonatlichSummiertModel>
    {
        private int jahrvon;
        private int jahrbis;
        private LineSeries<decimal> erhaltenSeries;
        private LineSeries<decimal> gesamtSeries;
        public ZinsenGesamtentwicklungMonatlichSummiertViewModel()
        {
            Title = "Auswertung Zinsen monatlich Summiert";
            jahrvon = GlobalUserVariables.JahrVon;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
            gesamtSeries = new LineSeries<decimal>();
            erhaltenSeries = new LineSeries<decimal>();
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/zinsen/Summiert/Monatlich?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<ZinsenGesamtentwicklungMonatlichSummiertModel>>();

                IList<decimal> GesamtChart = [];
                IList<decimal> ErhaltenChart = [];
                Labels = new string[ItemList.Count];
                int index = 0;

                ItemList.ToList().ForEach(a =>
                {
                    GesamtChart.Add(a.Gesamt);
                    ErhaltenChart.Add(a.Erhalten);
                    Labels[index] = a.Datum.ToString("MM.yyyy", CultureInfo.CurrentCulture);
                    index++;
                });

                gesamtSeries = new LineSeries<decimal>
                {
                    Values = GesamtChart,
                    Name = "Gesamt",
                };
                erhaltenSeries = new LineSeries<decimal>
                {
                    Values = ErhaltenChart,
                    Name = "Erhalten",
                };

                XAxes.First().Labels = Labels;
                XAxes.First().Name = "Monat";
                YAxes.First().Name = "Betrag";
                Series = new LineSeries<decimal>[2] { gesamtSeries, erhaltenSeries };

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

        public bool GesamtSeriesVisibility
        {
            get { return gesamtSeries.IsVisible; }
            set
            {
                gesamtSeries.IsVisible = value;
                OnPropertyChanged(nameof(Series));
            }
        }
        public bool ErhaltenSeriesVisibility
        {
            get { return erhaltenSeries.IsVisible; }
            set
            {
                erhaltenSeries.IsVisible = value;
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
