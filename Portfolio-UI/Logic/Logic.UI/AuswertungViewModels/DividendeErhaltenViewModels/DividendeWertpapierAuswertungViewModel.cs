using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.AuswertungModels;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace Logic.UI.AuswertungViewModels
{
    public class DividendeWertpapierAuswertungViewModel : ViewModelAuswertung<DividendeWertpapierAuswertungModel>
    {
        private int jahrvon;
        private int jahrbis;

        public DividendeWertpapierAuswertungViewModel()
        {
            Title = "Auswertung Dividende je Wertpapier";
            jahrvon = GlobalUserVariables.JahrVon;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        public async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividendenErhalten/Gesamt/Wertpapiere?jahrVon={jahrvon}&jahrBis={jahrbis}");
                if (resp.IsSuccessStatusCode)
                    ItemList = await resp.Content.ReadAsAsync<ObservableCollection<DividendeWertpapierAuswertungModel>>();

                PieSeries<double>[] series = new PieSeries<double>[ItemList.Count];

                int index = 0;
                ItemList.ToList().ForEach(a =>
                {
                    series.SetValue(new PieSeries<double> { Values = [a.Betrag], Name = a.Bezeichnung }, index);
                    index++;
                });

                Series = series;

                OnPropertyChanged(nameof(Series));
            }
            RequestIsWorking = false;
            OnPropertyChanged("ItemList");
        }

        #region Bindings
        public ICommand LoadDataCommand { get; set; }
        public int? JahrVon
        {
            get => jahrvon;
            set
            {
                ValidatZahl(value, nameof(JahrVon));
                this.OnPropertyChanged();
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
                this.OnPropertyChanged();
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
