using Aktien.Logic.Core.Validierung.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.AuswertungModels.SteueModels;
using LiveChartsCore.SkiaSharpView;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.AuswertungViewModels.SteuerViewModels
{
    public class SteuerGesamtentwicklungSummiertViewModel : ViewModelAuswertung<SteuerGesamtentwicklungSummiertModel>
    {
        private int jahrvon;
        private int jahrbis;
        public SteuerGesamtentwicklungSummiertViewModel()
        {
            Title = "Auswertung Steuer Summiert";
            jahrvon = GlobalUserVariables.JahrVon;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/steuern/Summiert/Steuer?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<SteuerGesamtentwicklungSummiertModel>>();

                IList<double> Values = new List<double>();
                Labels = new string[ItemList.Count];
                int index = 0;

                ItemList.ToList().ForEach(a =>
                {
                    Values.Add(a.Betrag);
                    Labels[index] = a.Datum.ToString("MM.yyyy", CultureInfo.CurrentCulture);
                    index++;
                });

                LineSeries<double> series = new LineSeries<double>
                {
                    Values = Values,
                    Name = "Betrag",
                    TooltipLabelFormatter = (point) => point.PrimaryValue.ToString("N2") + "€"
                };

                XAxes.First().Labels = Labels;
                XAxes.First().Name = "Monat";
                YAxes.First().Name = "Betrag";
                Series = new LineSeries<double>[1] { series };

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

        #endregion

        #region Validate
        private bool ValidatZahl(int? zahl, string fieldname)
        {
            BaseValidierung Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateAnzahl(zahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }
        #endregion

    }
}
