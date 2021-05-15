using Aktien.Data.Types;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.DividendeModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeErhaltenViewModel : ViewModelStammdaten<DividendeErhaltenModel>
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
            this.RaisePropertyChanged(nameof(DividendeText));
            DividendeID = id;
            this.betrag = betrag;
            BerechneGesamtWerte();
        }

        public async void Bearbeiten(int id)
        {
            LoadAktie = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync($"https://localhost:5001/api/DividendeErhalten/{id}");
                if (resp.IsSuccessStatusCode)
                {
                    data = await resp.Content.ReadAsAsync<DividendeErhaltenModel>();
                    Bestand = data.Bestand;
                    Quellensteuer = data.Quellensteuer;
                    Wechselkurs = data.Umrechnungskurs;
                    RundungTyp = data.RundungArt;
                    DividendeAusgewaehlt(data.DividendeID, data.Dividende_Betrag, data.Dividende_Zahldatum);
                    state = State.Bearbeiten;
                }
            }
            LoadAktie = false;
        }

        private int DividendeID
        { 
            set 
            {
                data.DividendeID = value;
                ValidateDividende(value);
                this.RaisePropertyChanged(nameof(DividendeText));
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
            this.RaisePropertyChanged(nameof(GesamtNetto));
            this.RaisePropertyChanged(nameof(GesamtBrutto));
            if (WechsellkursHasValue)
            {
                data.GesamtNettoUmgerechnetErhalten = new DividendenBerechnungen().BetragUmgerechnet(data.GesamtNetto, data.Umrechnungskurs,true, data.RundungArt);
                data.GesamtNettoUmgerechnetErmittelt = new DividendenBerechnungen().BetragUmgerechnet(data.GesamtNetto, data.Umrechnungskurs, false, data.RundungArt);
                this.RaisePropertyChanged(nameof(GesamtNettoUmgerechnet));
                this.RaisePropertyChanged(nameof(GesamtNettoUmgerechnetUngerundet));
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
                if (LoadAktie || this.data.Bestand != value)
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
                if (LoadAktie || this.data.Quellensteuer != value)
                {                   
                    this.data.Quellensteuer = value;
                    this.RaisePropertyChanged();
                    BerechneGesamtWerte();
                }
            }
        }
        public Double? Wechselkurs
        {
            get { return data.Umrechnungskurs; }
            set
            {
                if (LoadAktie || this.data.Umrechnungskurs != value)
                {  
                    this.data.Umrechnungskurs = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(WechsellkursHasValue));
                    ((DelegateCommand)OpenDividendeCommand).RaiseCanExecuteChanged();
                    BerechneGesamtWerte();
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
            Messenger.Default.Send<OpenDividendenAuswahlMessage>(new OpenDividendenAuswahlMessage(OpenDividendenAuswahlMessageCallback,data.WertpapierID), "DividendeErhalten");
        }
        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.PostAsJsonAsync($"https://localhost:5001/api/DividendeErhalten?state={state}", data);

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), GetStammdatenTyp());
                }
                else if (resp.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    SendExceptionMessage("Fehler - Dividende");
                    return;
                }
                else
                {
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                    return;
                }
            }
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
            data = new DividendeErhaltenModel();
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

