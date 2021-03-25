using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Core.DividendeLogic.Classes;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeErhaltenViewModel : ViewModelStammdaten<DividendeErhalten>
    {
        private string dividendetext;
        private Double betrag;

        public DividendeErhaltenViewModel()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            OpenAuswahlCommand = new RelayCommand(this.ExecuteOpenAuswahlCommand);
            OpenDividendeCommand = new DelegateCommand(this.ExecuteOpenDividendeCommand, this.CanExecuteOpenDividendeCommand);
            Cleanup();
        }

        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.dividendeErhalten;


        public void DividendeAusgewaehlt(int id, double betrag, DateTime datum)
        {
            DateTimeFormatInfo fmt = (new CultureInfo("de-DE")).DateTimeFormat;
            dividendetext = datum.ToString("d", fmt) + " (" + betrag.ToString("N2") + ")";
            this.RaisePropertyChanged("DividendeText");
            DividendeID = id;
            this.betrag = betrag;
            BerechneGesamtWerte();
        }

        public void Bearbeiten(int id)
        {
            var dividendeLoad = new DividendeErhaltenAPI().LadeAnhandID(id);

            data = new DividendeErhalten
            {
                ID = dividendeLoad.ID,
                WertpapierID = dividendeLoad.WertpapierID,
                DividendeID = dividendeLoad.DividendeID,
                GesamtBrutto = dividendeLoad.GesamtBrutto,
                GesamtNetto = dividendeLoad.GesamtNetto, 
            };

            Bestand = dividendeLoad.Bestand;
            Quellensteuer = dividendeLoad.Quellensteuer;
            Wechselkurs = dividendeLoad.Umrechnungskurs;
            RundungTyp = dividendeLoad.RundungArt;
            DividendeAusgewaehlt(dividendeLoad.Dividende.ID, dividendeLoad.Dividende.Betrag, dividendeLoad.Dividende.Zahldatum);
            state = State.Bearbeiten;
        }

        private int DividendeID
        { 
            set 
            {
                data.DividendeID = value;
                ValidateDividende(value);
                this.RaisePropertyChanged("DividendeText");
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                if (OpenDividendeCommand != null)
                    ((DelegateCommand)OpenDividendeCommand).RaiseCanExecuteChanged();
            } 
        }

  
        public void WertpapierID(int wertpapierID)
        {
            data.WertpapierID = wertpapierID;
        }

        public void BerechneGesamtWerte()
        {
            data.GesamtBrutto = new DividendenBerechnungen().GesamtBrutto(betrag, data.Bestand);
            data.GesamtNetto = new DividendenBerechnungen().GesamtNetto(data.GesamtBrutto, data.Quellensteuer.GetValueOrDefault(0));
            this.RaisePropertyChanged("GesamtNetto");
            this.RaisePropertyChanged("GesamtBrutto");
            if (WechsellkursHasValue)
            {
                data.GesamtNettoUmgerechnetErhalten = new DividendenBerechnungen().BetragUmgerechnet(data.GesamtNetto, data.Umrechnungskurs,true, data.RundungArt);
                data.GesamtNettoUmgerechnetErmittelt = new DividendenBerechnungen().BetragUmgerechnet(data.GesamtNetto, data.Umrechnungskurs, false, data.RundungArt);
                this.RaisePropertyChanged("GesamtNettoUmgerechnet");
                this.RaisePropertyChanged("GesamtNettoUmgerechnetUngerundet");
            }
        }

        #region Bindings
        public ICommand OpenAuswahlCommand { get; set; }
        public ICommand OpenDividendeCommand { get; set; }
        public Double? Bestand
        {
            get 
            {
                if (data.Bestand == -1)
                    return null;
                else
                    return data.Bestand; 
            }
            set 
            {
                if (this.data.Bestand != value)
                {
                    ValidateBestand(value);
                    this.data.Bestand = value.GetValueOrDefault(-1);
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                    BerechneGesamtWerte();
                }
            }
        }
        public string DividendeText
        {
            get { return dividendetext; }
        }
        public Double? Quellensteuer
        {
            get { return data.Quellensteuer; }
            set
            {
                if (this.data.Quellensteuer != value)
                {
                    BerechneGesamtWerte();
                    this.data.Quellensteuer = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public Double? Wechselkurs
        {
            get { return data.Umrechnungskurs; }
            set
            {
                if (this.data.Umrechnungskurs != value)
                {
                    this.data.Umrechnungskurs = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged("WechsellkursHasValue");
                }
            }
        }

        public IEnumerable<DividendenRundungTypes> RundungTypes
        {
            get
            {
                return Enum.GetValues(typeof(DividendenRundungTypes)).Cast<DividendenRundungTypes>();
            }
        }

        public DividendenRundungTypes RundungTyp
        {
            get { return data.RundungArt; }
            set
            {
                if (LoadAktie || (this.data.RundungArt != value))
                {
                    this.data.RundungArt = value;
                    this.RaisePropertyChanged();
                    BerechneGesamtWerte();
                }
            }
        }

        public Double? GesamtBrutto
        {
            get { return data.GesamtBrutto; }
        }
        public Double? GesamtNetto
        {
            get { return data.GesamtNetto; }
        }
        public Double? GesamtNettoUmgerechnet
        {
            get { return data.GesamtNettoUmgerechnetErhalten.GetValueOrDefault(0); }
        }

        public Double? GesamtNettoUmgerechnetUngerundet
        {
            get { return data.GesamtNettoUmgerechnetErmittelt.GetValueOrDefault(0); }
        }

        public bool WechsellkursHasValue { get { return this.data.Umrechnungskurs.GetValueOrDefault(0)>0; } }
        #endregion

        #region Commands
        private void ExecuteOpenAuswahlCommand()
        {
            Messenger.Default.Send<OpenDividendenAuswahlMessage>(new OpenDividendenAuswahlMessage(OpenDividendenAuswahlMessageCallback,data.WertpapierID), "DatenAnpassung");
        }
        protected override void ExecuteSaveCommand()
        {
            var API = new DepotAPI();
            if (state == State.Neu)
            {
                API.NeueDividendeErhalten(data);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Erhaltene Dividende gespeichert." }, GetStammdatenTyp());

            }
            else
            {
                API.AktualisiereDividendeErhalten(data);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Erhaltene Dividende aktualisiert." }, GetStammdatenTyp());
            }
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.dividendeErhalten);
        }
        private bool CanExecuteOpenDividendeCommand()
        {
            return (data.DividendeID != -1) && (data.Umrechnungskurs.GetValueOrDefault(0)>0);
        }
        private void ExecuteOpenDividendeCommand()
        {
            Messenger.Default.Send<OpenDividendeProStueckAnpassenMessage>(new OpenDividendeProStueckAnpassenMessage { DividendeID = data.DividendeID,  Umrechnungskurs = data.Umrechnungskurs.Value });
        }
        #endregion

        #region Callbacks
        private void OpenDividendenAuswahlMessageCallback(bool confirmed, int id, double betrag, DateTime date)
        {
            if (confirmed)
                DividendeAusgewaehlt(id, betrag, date);
        }
        #endregion

        #region Validate
        private bool ValidateBestand(Double? bestand)
        {
            var Validierung = new DividendeErhaltenValidierung();

            bool isValid = Validierung.ValidateBestand(bestand, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Bestand", validationErrors);
            return isValid;
        }

        private bool ValidateDividende(int id)
        {
            var Validierung = new DividendeErhaltenValidierung();

            bool isValid = Validierung.ValidateDividende(id, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "DividendeText", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            data = new DividendeErhalten();
            dividendetext = "";
            DividendeID = -1;
            Bestand = -1;
            state = State.Neu;
            Quellensteuer = null;
            Wechselkurs = null;
            RundungTyp = DividendenRundungTypes.Normal;
            betrag = 0;
            this.RaisePropertyChanged();
        }
    }
}

