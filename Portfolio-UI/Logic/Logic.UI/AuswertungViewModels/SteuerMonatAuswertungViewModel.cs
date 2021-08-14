using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Data.Model.AuswertungModels;
using LiveCharts;
using LiveCharts.Wpf;
using Logic.UI.BaseViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.AuswertungViewModels
{
    public class SteuerMonatAuswertungViewModel : ViewModelAuswertung<SteuerMonatAuswertungModel>
    {
        private int jahrvon;
        private int jahrbis;
        public SteuerMonatAuswertungViewModel()
        {
            Title = "Auswertung Steuer je Monat";
            jahrvon = DateTime.Now.Year;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(this.ExcecuteLoadDataCommand, this.CanExcecuteLoadDataCommand);
            Formatter = value => string.Format("{0:N2}€", value);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/steuern/Monate?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<SteuerMonatAuswertungModel>>();

                ChartValues<double> values = new ChartValues<double>();
                Labels = new string[ItemList.Count];
                int index = 0;

                ItemList.ToList().ForEach(a =>
                {
                    values.Add(a.Betrag);
                    Labels[index] = a.Datum.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
                    index++;
                });
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries{ Values = values, Title="Betrag" }
                };

                RaisePropertyChanged(nameof(SeriesCollection));
                RaisePropertyChanged(nameof(Labels));
                RaisePropertyChanged(nameof(Formatter));
            }
        }


        #region Bindings
        public ICommand LoadDataCommand { get; set; }
        public int? JahrVon
        {
            get => jahrvon;
            set
            {
                ValidatZahl(value, nameof(JahrVon));
                this.RaisePropertyChanged();
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
                this.RaisePropertyChanged();
                ((DelegateCommand)LoadDataCommand).RaiseCanExecuteChanged();
                jahrbis = value.GetValueOrDefault(0);
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
