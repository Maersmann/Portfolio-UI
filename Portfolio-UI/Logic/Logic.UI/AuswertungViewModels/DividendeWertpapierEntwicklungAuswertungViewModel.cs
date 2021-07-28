using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.AuswahlMessages;
using Data.Model.AuswertungModels;
using Data.Types.AuswertungTypes;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
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
    public class DividendeWertpapierEntwicklungAuswertungViewModel : ViewModelAuswertung<DividendeWertpapierEntwicklungAuswertungModel>
    {
        private int jahrvon;
        private int jahrbis;
        private DividendenBetragTyp typ;
        private int wertpapierID;
        public DividendeWertpapierEntwicklungAuswertungViewModel()
        {
            Data = new DividendeWertpapierEntwicklungAuswertungModel();
            Title = "Auswertung Entwicklung Dividende Wertpapier";
            jahrvon = DateTime.Now.Year;
            jahrbis = DateTime.Now.Year;
            typ = DividendenBetragTyp.NachSteuer;
            wertpapierID = 0;
            AuswahlCommand = new RelayCommand(() => ExcecuteAuswahlCommand());
            Formatter = value => value.ToString("0.## €");
        }

        private void ExcecuteAuswahlCommand()
        {
            Messenger.Default.Send<OpenWertpapierAuswahlMessage>(new OpenWertpapierAuswahlMessage(OpenOpenWertpapierAuswahlMessageCallback), "DividendeWertpapierEntwicklung");
        }

        private async void LoadData()
        {
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividenden/Wertpapiere/{wertpapierID}/Entwicklung?jahrVon={jahrvon}&jahrBis={jahrbis}&typ={typ}");
            if (resp.IsSuccessStatusCode)
            {
                Data = await resp.Content.ReadAsAsync<DividendeWertpapierEntwicklungAuswertungModel>();

                ChartValues<double> values = new ChartValues<double>();
                Labels = new string[Data.Betraege.Count];
                int index = 0;

                Data.Betraege.ToList().ForEach(a =>
                {
                    values.Add(a.Betrag);
                    Labels[index] = a.Datum.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
                    index++;
                });
                SeriesCollection = new SeriesCollection
                {
                    new LineSeries{ Values = values, Title="Betrag" }
                };

                RaisePropertyChanged(nameof(Data));
                RaisePropertyChanged(nameof(SeriesCollection));
                RaisePropertyChanged(nameof(Labels));
                RaisePropertyChanged(nameof(Formatter));
            }
        }
        #region Callbacks
        private void OpenOpenWertpapierAuswahlMessageCallback(bool confirmed, int id)
        {
            if (confirmed)
            {
                wertpapierID = id;
                LoadData();
            }
        }
        #endregion

        #region Bindings
        public int? JahrVon
        {
            get => jahrvon;
            set
            {
                ValidatZahl(value, nameof(JahrVon));
                this.RaisePropertyChanged();
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
                jahrbis = value.GetValueOrDefault(0);
            }
        }
        public DividendenBetragTyp Typ 
        {
            get => typ;
            set
            {
                this.RaisePropertyChanged();
                typ = value;
            }
        }
        public ICommand AuswahlCommand { get; set; }

        public static IEnumerable<DividendenBetragTyp> Types => Enum.GetValues(typeof(DividendenBetragTyp)).Cast<DividendenBetragTyp>();
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
