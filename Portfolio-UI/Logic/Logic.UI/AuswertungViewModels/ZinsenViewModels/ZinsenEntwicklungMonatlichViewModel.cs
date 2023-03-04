using Aktien.Logic.Core.Validierung.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.AuswertungModels;
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
    public class ZinsenEntwicklungMonatlichViewModel : ViewModelAuswertung<ZinsenEntwicklungMonatlichModel>
    {
        private int jahrvon;
        private int jahrbis;
        private LineSeries<decimal> gesamtSeries;
        private LineSeries<decimal> erhaltenSeries;
        public ZinsenEntwicklungMonatlichViewModel()
        {
            Title = "Auswertung Zinsen je Monat";
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
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/zinsen/Gesamt/Monat/Entwicklung?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<ZinsenEntwicklungMonatlichModel>>();

                IList<decimal> GesamtChart = new List<decimal>();
                IList<decimal> ErhaltenChart = new List<decimal>();
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
                    TooltipLabelFormatter = (point) => "Gesamt " + point.PrimaryValue.ToString("N2") + "€"
                };
                erhaltenSeries = new LineSeries<decimal>
                {
                    Values = ErhaltenChart,
                    Name = "Erhalten",
                    TooltipLabelFormatter = (point) => "Erhalten " + point.PrimaryValue.ToString("N2") + "€"
                };

                XAxes.First().Labels = Labels;
                XAxes.First().Name = "Monat";
                YAxes.First().Name = "Betrag";
                Series = new LineSeries<decimal>[2] { erhaltenSeries, gesamtSeries };

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

        public bool GesamtSeriesVisibility
        {
            get { return gesamtSeries.IsVisible; }
            set
            {
                gesamtSeries.IsVisible = value;
                RaisePropertyChanged(nameof(Series));
            }
        }
        public bool ErhaltenSeriesVisibility
        {
            get { return erhaltenSeries.IsVisible; }
            set
            {
                erhaltenSeries.IsVisible = value;
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
