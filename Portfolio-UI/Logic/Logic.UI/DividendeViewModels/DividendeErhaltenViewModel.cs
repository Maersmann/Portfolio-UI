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
using Data.Model.SteuerModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Logic.Core.SteuernLogic;
using Logic.Messages.SteuernMessages;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeErhaltenViewModel : ViewModelStammdaten<DividendeErhaltenStammdatenModel>
    {
        private string dividendetext;
        private Double betrag;
        private bool neueDividendeNichtGespeichert;

        public DividendeErhaltenViewModel()
        {
            
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            OpenAuswahlCommand = new RelayCommand(this.ExecuteOpenAuswahlCommand);
            OpenSteuernCommand = new RelayCommand(this.ExecuteOpenSteuernCommand);
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
                    data = await resp.Content.ReadAsAsync<DividendeErhaltenStammdatenModel>();
                    Bestand = data.Bestand;
                    Wechselkurs = data.Umrechnungskurs;
                    RundungTyp = data.RundungArt;
                    DividendeAusgewaehlt(data.DividendeID, data.Dividende.Betrag, data.Dividende.Zahldatum);
                    state = State.Bearbeiten;
                    neueDividendeNichtGespeichert = false;
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
            data.Bemessungsgrundlage = new DividendenBerechnungen().Bemessungsgrundlage(betrag, data.Bestand);
            data.Erhalten = data.Bemessungsgrundlage;
            data.SteuernVorZwischensumme = 0;
            data.SteuernNachZwischensumme = 0;

            if (data.Steuer != null)
            { 
                data.SteuernVorZwischensumme = new SteuerBerechnen().SteuernVorZwischensumme(data.Steuer.Steuern);
                data.SteuernNachZwischensumme = new SteuerBerechnen().SteuernNachZwischensumme(data.Steuer.Steuern);
            }
            data.Zwischensumme = data.Bemessungsgrundlage + data.SteuernVorZwischensumme;
            data.Erhalten = data.Zwischensumme.GetValueOrDefault(0) + data.SteuernNachZwischensumme.GetValueOrDefault(0);

            if (WechsellkursHasValue)
            {
                data.ErhaltenUmgerechnetErmittelt = new DividendenBerechnungen().BetragUmgerechnet(data.Erhalten, data.Umrechnungskurs, false, data.RundungArt);
                data.ZwischensummeUmgerechnet = new DividendenBerechnungen().BetragUmgerechnet(data.Zwischensumme.GetValueOrDefault(0), data.Umrechnungskurs, true, data.RundungArt);
                
                data.Erhalten = data.ZwischensummeUmgerechnet.GetValueOrDefault(0) + data.SteuernNachZwischensumme.GetValueOrDefault(0);          
            }

            this.RaisePropertyChanged(nameof(ZwischensummeTxt));
            this.RaisePropertyChanged(nameof(Bemessungsgrundlage));
            this.RaisePropertyChanged(nameof(Erhalten));
            this.RaisePropertyChanged(nameof(SteuerNachZwischensumme));
            this.RaisePropertyChanged(nameof(SteuerVorZwischensumme));
            this.RaisePropertyChanged(nameof(Zwischensumme));
            this.RaisePropertyChanged(nameof(ErhaltenUmgerechnetUngerundet));
            this.RaisePropertyChanged(nameof(ZwischensummeUmgerechnet));
        }

        #region Bindings
        public ICommand OpenAuswahlCommand { get; set; }
        public ICommand OpenDividendeCommand { get; set; }
        public ICommand OpenSteuernCommand { get; set; }
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

        public double? Bemessungsgrundlage => data.Bemessungsgrundlage;
        public double? Erhalten => data.Erhalten;
        public double? ErhaltenUmgerechnetUngerundet => data.ErhaltenUmgerechnetErmittelt.GetValueOrDefault(0);
        public double? SteuerVorZwischensumme => data.SteuernVorZwischensumme.GetValueOrDefault(0);
        public double? SteuerNachZwischensumme => data.SteuernNachZwischensumme.GetValueOrDefault(0);
        public double? Zwischensumme => data.Zwischensumme.GetValueOrDefault(0);
        public double? ZwischensummeUmgerechnet => data.ZwischensummeUmgerechnet.GetValueOrDefault(0);
        public string ZwischensummeTxt 
        { 
            get 
            {
                if (WechsellkursHasValue)
                    return data.Zwischensumme.GetValueOrDefault(0).ToString("N2") + " / " + data.ZwischensummeUmgerechnet.GetValueOrDefault(0).ToString("N2");
                else
                    return data.Zwischensumme.GetValueOrDefault(0).ToString("N2");
            } 
        }

        public bool WechsellkursHasValue => data.Umrechnungskurs.GetValueOrDefault(0) > 0;

        
        #endregion

        #region Commands
        private void ExecuteOpenAuswahlCommand()
        {
            Messenger.Default.Send<OpenDividendenAuswahlMessage>(new OpenDividendenAuswahlMessage(OpenDividendenAuswahlMessageCallback,data.WertpapierID), "DividendeErhalten");
        }

        private void ExecuteOpenSteuernCommand()
        {
            Messenger.Default.Send<OpenSteuernUebersichtMessage>(new OpenSteuernUebersichtMessage(OpenSteuernUebersichtMessageCallback, data.SteuergruppeID, !neueDividendeNichtGespeichert), "DividendeErhalten");
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.PostAsJsonAsync($"https://localhost:5001/api/DividendeErhalten?state={state}", data);

                if (resp.IsSuccessStatusCode)
                {
                    neueDividendeNichtGespeichert = false;
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), GetStammdatenTyp());
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
        protected override async void ExecuteCloseCommand()
        {         
            if (data.SteuergruppeID.HasValue && state.Equals(State.Neu) && neueDividendeNichtGespeichert)
            {
                if (GlobalVariables.ServerIsOnline)
                {
                    HttpResponseMessage resp = await Client.DeleteAsync($"https://localhost:5001/api/dividendeErhalten/Steuern/{data.SteuergruppeID}");
                    if (!resp.IsSuccessStatusCode)
                    {
                        SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                        return;
                    }

                }
            }
            base.ExecuteCloseCommand();
        }

        #endregion

        #region Callbacks
        private void OpenDividendenAuswahlMessageCallback(bool confirmed, int id, double betrag, DateTime date)
        {
            if (confirmed)
                DividendeAusgewaehlt(id, betrag, date);
        }
        private async void OpenSteuernUebersichtMessageCallback(bool confirmed, int id)
        {
            if (confirmed)
            { 
                data.SteuergruppeID = id;
                if (GlobalVariables.ServerIsOnline)
                {
                    HttpResponseMessage resp = await Client.GetAsync($"https://localhost:5001/api/Steuern?steuergruppeid={id}");
                    if (resp.IsSuccessStatusCode)
                        data.Steuer.Steuern = await resp.Content.ReadAsAsync<ObservableCollection<SteuerModel>>();
                    else
                        SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                }
                BerechneGesamtWerte();
            }
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
            neueDividendeNichtGespeichert = true;
            data = new DividendeErhaltenStammdatenModel { Steuer = new SteuergruppeModel { Steuern = new List<SteuerModel>() } };
            dividendetext = "";
            DividendeID = -1;
            Bestand = -1;
            state = State.Neu;
            Wechselkurs = null;
            RundungTyp = DividendenRundungTypes.Normal;
            betrag = 0;
            this.RaisePropertyChanged();
        }

       
    }
}

