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

namespace Logic.UI.AuswertungViewModels
{
    public class SteuerMonatJahresVergleichAuswertungViewModel : ViewModelAuswertung<SteuerMonatJahresVergleichAuswertungModel>
    {
        private int jahrvon;
        private int jahrbis;
        public SteuerMonatJahresVergleichAuswertungViewModel()
        {
            Title = "Auswertung Steuerart je Monat - Jahresvergleich";
            jahrvon = DateTime.Now.Year;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(this.ExcecuteLoadDataCommand, this.CanExcecuteLoadDataCommand);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/steuern/Monate/Jahresvergleich?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<SteuerMonatJahresVergleichAuswertungModel>>();

                Labels = new string[12];
                ColumnSeries<double>[] series = new ColumnSeries<double>[ItemList.Count];
                int index = 0;
                ItemList.ToList().ForEach(item =>
                {
                    ColumnSeries<double> coloumn = new ColumnSeries<double>
                    {
                        Name = item.Jahr.ToString(),
                        Values = new List<double>(),
                        TooltipLabelFormatter = (point) => item.Jahr + " " + point.PrimaryValue.ToString("N2") + "€"
                    };

                    var betraege = new List<double>();
                    item.Monatswerte.ToList().ForEach(mw =>
                    {
                        betraege.Add(mw.Betrag);
                    });
                    coloumn.Values = betraege;
                    series.SetValue(coloumn, index);
                    index++;
                });

                for (int monat = 1; monat <= 12; monat++)
                {
                    Labels[monat - 1] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monat);
                }

                XAxes.First().Labels = Labels;
                XAxes.First().Name = "Monat";
                YAxes.First().Name = "Betrag";

                Series = series;

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
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateAnzahl(zahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }
        #endregion
    }
}
