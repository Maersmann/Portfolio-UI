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

namespace Logic.UI.AuswertungViewModels.ZinsenViewModels
{
    public class ZinsenGesamtentwicklungJaehrlichSummiertViewModel : ViewModelAuswertung<ZinsenGesamtentwicklungJaehrlichSummiertModel>
    {
        private int jahrvon;
        private int jahrbis;
        private LineSeries<decimal> erhaltenSeries;
        private LineSeries<decimal> gesamtSeries;
        public ZinsenGesamtentwicklungJaehrlichSummiertViewModel()
        {
            Title = "Auswertung Zinsen jährlich Summiert";
            jahrvon = GlobalUserVariables.JahrVon;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
            erhaltenSeries = new LineSeries<decimal>();
            gesamtSeries = new LineSeries<decimal>();
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/zinsen/Summiert/Jaehrlich?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<ZinsenGesamtentwicklungJaehrlichSummiertModel>>();

                IList<decimal> GesamtChart = [];
                IList<decimal> ErhaltenChart = [];
                Labels = new string[ItemList.Count];
                int index = 0;

                ItemList.ToList().ForEach(a =>
                {
                    ErhaltenChart.Add(a.Erhalten);
                    GesamtChart.Add(a.Gesamt);
                    Labels[index] = a.Jahr.ToString();
                    index++;
                });

                erhaltenSeries = new LineSeries<decimal>
                {
                    Values = ErhaltenChart,
                    Name = "Erhalten",
                };
                gesamtSeries = new LineSeries<decimal>
                {
                    Values = GesamtChart,
                    Name = "Gesamt",
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

        public bool ErhaltenSeriesVisibility
        {
            get { return erhaltenSeries.IsVisible; }
            set
            {
                erhaltenSeries.IsVisible = value;
                OnPropertyChanged(nameof(Series));
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
