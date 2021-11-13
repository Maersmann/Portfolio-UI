﻿using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Data.Model.AuswertungModels;
using LiveCharts;
using LiveCharts.Wpf;
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
using Data.Types.AuswertungTypes;

namespace Logic.UI.AuswertungViewModels
{
    public class DividendeVergleichMonatViewModel : ViewModelAuswertung<DividendeVergleichMonatModel>
    {
        private int jahrvon;
        private int jahrbis;
        private DividendenBetragTyp typ;

        public DividendeVergleichMonatViewModel()
        {
            Title = "Auswertung Dividende je Monat - Jahresvergleich";
            jahrvon = DateTime.Now.Year;
            jahrbis = DateTime.Now.Year;
            typ = DividendenBetragTyp.Netto;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
            Formatter = value => string.Format("{0:N2}€", value);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividendenErhalten/Gesamt/Monat/Vergleich?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<DividendeVergleichMonatModel>>();
                SetDataIntoChart();


            }
            RequestIsWorking = false;
        }

        private void SetDataIntoChart()
        {
            Labels = new string[12];
            SeriesCollection = new SeriesCollection();
            ItemList.ToList().ForEach(item =>
            {
                ColumnSeries coloumn = new ColumnSeries
                {
                    Title = item.Jahr.ToString(),
                    Values = new ChartValues<double>()
                };
                item.Monatswerte.ToList().ForEach(mw =>
                {
                    Double Betrag = typ.Equals(DividendenBetragTyp.Brutto) ? mw.Brutto : mw.Netto;
                    coloumn.Values.Add(Betrag);
                    if (Betrag > HighestValue)
                    {
                        HighestValue = Betrag;
                    }
                });

                SeriesCollection.Add(coloumn);
            });

            BerechneSeperator();

            for (int monat = 1; monat <= 12; monat++)
            {
                Labels[monat - 1] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monat);
            }

            RaisePropertyChanged(nameof(SeriesCollection));
            RaisePropertyChanged(nameof(Labels));
            RaisePropertyChanged(nameof(Formatter));
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

        public static IEnumerable<DividendenBetragTyp> Types => Enum.GetValues(typeof(DividendenBetragTyp)).Cast<DividendenBetragTyp>();
        public DividendenBetragTyp Typ
        {
            get => typ;
            set
            {
                RaisePropertyChanged();
                typ = value;
                SetDataIntoChart();
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