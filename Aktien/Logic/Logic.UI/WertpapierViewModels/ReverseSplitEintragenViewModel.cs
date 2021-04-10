using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.Core.DepotLogic.Classes;
using Aktien.Logic.Core.Validierungen;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Aktien.Logic.UI.WertpapierViewModels
{
    public class ReverseSplitEintragenViewModel : ViewModelValidate
    {
        private DepotWertpapier alteAktie;
        private readonly DepotWertpapier neueAktie;
        private int verhaeltnis;

        public ReverseSplitEintragenViewModel()
        {
            Title = "Reverse-Split eintragen";
            alteAktie = new DepotWertpapier { Wertpapier = new Wertpapier() };
            neueAktie = new DepotWertpapier { Wertpapier = new Wertpapier() };
            OpenAuswahlCommand = new RelayCommand(this.ExecuteOpenAuswahlCommand);
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Verhaeltnis = 1;
            ValidateNeueAktie("");

        }

        public int DepotWertpapierID
        {
            set
            {
                alteAktie = new DepotWertpapierAPI().LadeByWertpapierID(value);                
                this.RaisePropertyChanged("AlteAktieText");
            }
        }

        public void BerechneWerte()
        {
            neueAktie.Anzahl = Math.Round( alteAktie.Anzahl / verhaeltnis,3, MidpointRounding.AwayFromZero);
            neueAktie.BuyIn = new KaufBerechnungen().BuyInAktieGekauft(0, 0, neueAktie.Anzahl, (alteAktie.BuyIn * verhaeltnis), NeueAnzahl, 0, Data.Types.WertpapierTypes.OrderTypes.Normal);
            if (Double.IsNaN(neueAktie.BuyIn)) neueAktie.BuyIn = 0;
            this.RaisePropertyChanged("NeueAnzahl");
            this.RaisePropertyChanged("NeuerBuyIn");
        }

        #region Bindings

        public string AlteAktieText => alteAktie.Wertpapier.Name;
        public string NeueAktieText => neueAktie.Wertpapier.Name;
        public Double NeueAnzahl => neueAktie.Anzahl;
        public Double NeuerBuyIn => neueAktie.BuyIn;
        public ICommand OpenAuswahlCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public int? Verhaeltnis
        {
            get => this.verhaeltnis;
            set
            {
                verhaeltnis = value.GetValueOrDefault(0);
                ValidateVerhaeltnis(verhaeltnis);
                BerechneWerte();
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        private void ExecuteOpenAuswahlCommand()
        {
            Messenger.Default.Send<OpenWertpapierAuswahlMessage>(new OpenWertpapierAuswahlMessage(OpenAktieMessageCallback) { WertpapierTypes = Data.Types.WertpapierTypes.WertpapierTypes.Aktie}, "ReverseSplitEintragen");
        }

        private bool CanExecuteSaveCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private void ExecuteSaveCommand()
        {           
            new DepotAPI().NeuerReverseSplit(neueAktie, alteAktie);
            SendInformationMessage("Gespeichert");
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.aktien);
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.buysell);
            Messenger.Default.Send<CloseViewMessage>(new CloseViewMessage(), "ReverseSplitEintragen");
        }

        #endregion

        #region Callbacks
        private void OpenAktieMessageCallback(bool confirmed, int id)
        {
            if (confirmed)
            {
                neueAktie.Wertpapier = new AktieAPI().Lade(id);
                ValidateNeueAktie(neueAktie.Wertpapier.Name);
                BerechneWerte();
                this.RaisePropertyChanged("NeueAktieText");
            }
        }
        #endregion

        #region Validierungen
        private bool ValidateNeueAktie(string bezeichnung)
        {
            var Validierung = new ReverseSplitEintragenValidierung();

            bool isValid = Validierung.ValidateAktie(bezeichnung, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "NeueAktieText", validationErrors);
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            return isValid;
        }

        private bool ValidateVerhaeltnis(int verhaeltnis)
        {
            var Validierung = new ReverseSplitEintragenValidierung();

            bool isValid = Validierung.ValidateAnzahl(verhaeltnis, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Verhaeltnis", validationErrors);
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            return isValid;
        }

        #endregion
    }
}
