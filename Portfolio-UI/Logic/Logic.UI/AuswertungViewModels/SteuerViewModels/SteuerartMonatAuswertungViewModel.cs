using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Data.Model.AuswertungModels;
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
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;

namespace Logic.UI.AuswertungViewModels
{
    public class SteuerartMonatAuswertungViewModel : ViewModelAuswertung<SteuerartMonatAuswertungModel>
    {
        private int jahrvon;
        private int jahrbis;
        public SteuerartMonatAuswertungViewModel()
        {
            Title = "Auswertung Steuerart je Monat";
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
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/steuern/Steuerart/Monate?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<SteuerartMonatAuswertungModel>>();

                Labels = new string[ItemList.Count];
                IList<Betrag> werte = [];
                int index = 0;
                ItemList.ToList().ForEach(item =>
                {
                    item.Steuerarten.ToList().ForEach(steuer =>{
                        var wert = werte.ToList().Find(wert => wert.Steuerart.Equals(steuer.Steuerart));
                        if (wert == null)
                        {
                            wert = new Betrag
                            {
                                Betraege = [],
                                Steuerart = steuer.Steuerart
                            };
                            werte.Add(wert);
                        }
                        wert.Betraege.Add(steuer.Betrag);
                    });

                    Labels[index] = item.Datum.ToString("MM.yyyy", CultureInfo.CurrentCulture);                  
                    index++;
                });


                StackedColumnSeries<double>[] series = new StackedColumnSeries<double>[werte.Count];
                index = 0;
                werte.ToList().ForEach(wert =>
                {
                    var StackedColoumn = new StackedColumnSeries<double>
                    {
                        Values = wert.Betraege,
                        Name = wert.Steuerart,
                    };
                    series.SetValue(StackedColoumn, index);
                    index++;
                });

                

                XAxes.First().Labels = Labels;
                XAxes.First().Name = "Monat";
                YAxes.First().Name = "Betrag";

                Series = series;

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


    class Betrag {
        public IList<double> Betraege { get; set; }
        public string Steuerart { get; set; }
    }
}
