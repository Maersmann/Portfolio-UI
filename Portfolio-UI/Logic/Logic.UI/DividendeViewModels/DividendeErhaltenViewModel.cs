using Aktien.Data.Types;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using Data.Model.DividendeModels;
using Data.Model.SteuerModels;
using Data.Types.SteuerTypes;

using CommunityToolkit.Mvvm.Messaging;
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
using CommunityToolkit.Mvvm.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeErhaltenViewModel : ViewModelStammdaten<DividendeErhaltenStammdatenModel, StammdatenTypes>
    {
        private string bestand;
        private string wechselkurs;
        private double betrag;
        public DividendeErhaltenViewModel()
        {
            Title = "Informationen erhaltene Dividende";
            OpenAuswahlCommand = new RelayCommand(ExecuteOpenAuswahlCommand);
            OpenSteuernCommand = new RelayCommand(ExecuteOpenSteuernCommand);
            OpenDividendeCommand = new DelegateCommand(ExecuteOpenDividendeCommand, CanExecuteOpenDividendeCommand);
        }

        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.dividendeErhalten;


        public void DividendeAusgewaehlt(int id, double betrag, DateTime datum)
        {
            DateTimeFormatInfo fmt = (new CultureInfo("de-DE")).DateTimeFormat;
            DividendeText = datum.ToString("d", fmt) + " (" + betrag.ToString("N2") + ")";
            OnPropertyChanged(nameof(DividendeText));
            DividendeID = id;
            LadeAnzahlWertpapier();
            this.betrag = betrag;
            BerechneGesamtWerte();
        }

        public async void Bearbeiten(int id)
        {
            RequestIsWorking = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/DividendeErhalten/{id}");
                if (resp.IsSuccessStatusCode)
                {
                    Response = await resp.Content.ReadAsAsync<Response<DividendeErhaltenStammdatenModel>>();
                    Bestand = Data.Umrechnungskurs.ToString();
                    Wechselkurs = Data.Umrechnungskurs.ToString();
                    RundungTyp = Data.RundungArt;
                    DividendeAusgewaehlt(Data.DividendeID, Data.Dividende.Betrag, Data.Dividende.Zahldatum);
                    state = State.Bearbeiten;
                }
            }
            RequestIsWorking = false;
        }

        private async void LadeAnzahlWertpapier()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/depot/wertpapier/{Data.WertpapierID}/Anzahl");
                if (resp.IsSuccessStatusCode)
                {
                    Response<double> AnzahlResponse = await resp.Content.ReadAsAsync<Response<double>>();
                    Bestand = AnzahlResponse.Data.ToString();
                }
                RequestIsWorking = false;
            }
        }

        private int DividendeID
        { 
            set 
            {
                Data.DividendeID = value;
                ValidateDividende(value);
                this.OnPropertyChanged(nameof(DividendeText));
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                if (OpenDividendeCommand != null)
                    ((DelegateCommand)OpenDividendeCommand).RaiseCanExecuteChanged();
            } 
        }
  
        public void WertpapierID(int wertpapierID)
        {
            Data.WertpapierID = wertpapierID;
        }

        public void BerechneGesamtWerte()
        {
            Data.Bemessungsgrundlage = new DividendenBerechnungen().Bemessungsgrundlage(betrag, Data.Bestand);
            Data.Erhalten = Data.Bemessungsgrundlage;
            Data.SteuernVorZwischensumme = 0;
            Data.SteuernNachZwischensumme = 0;

            if (Data.Steuer != null)
            {
                Data.SteuernVorZwischensumme = new SteuerBerechnen().SteuernVorZwischensumme(Data.Steuer.Steuern);
                Data.SteuernNachZwischensumme = new SteuerBerechnen().SteuernNachZwischensumme(Data.Steuer.Steuern);
            }
            Data.Zwischensumme = Data.Bemessungsgrundlage + Data.SteuernVorZwischensumme;
            Data.Erhalten = Data.Zwischensumme.GetValueOrDefault(0) + Data.SteuernNachZwischensumme.GetValueOrDefault(0);

            if (WechsellkursHasValue)
            {
                Data.ErhaltenUmgerechnetErmittelt = new DividendenBerechnungen().BetragUmgerechnet(Data.Erhalten, Data.Umrechnungskurs, false, Data.RundungArt);
                Data.ZwischensummeUmgerechnet = new DividendenBerechnungen().BetragUmgerechnet(Data.Zwischensumme.GetValueOrDefault(0), Data.Umrechnungskurs, true, Data.RundungArt);

                Data.Erhalten = Data.ZwischensummeUmgerechnet.GetValueOrDefault(0) + Data.SteuernNachZwischensumme.GetValueOrDefault(0);       
            }

            OnPropertyChanged(nameof(ZwischensummeTxt));
            OnPropertyChanged(nameof(Bemessungsgrundlage));
            OnPropertyChanged(nameof(Erhalten));
            OnPropertyChanged(nameof(SteuerNachZwischensumme));
            OnPropertyChanged(nameof(SteuerVorZwischensumme));
            OnPropertyChanged(nameof(Zwischensumme));
            OnPropertyChanged(nameof(ErhaltenUmgerechnetUngerundet));
            OnPropertyChanged(nameof(ZwischensummeUmgerechnet));
        }

        #region Bindings
        public ICommand OpenAuswahlCommand { get; set; }
        public ICommand OpenDividendeCommand { get; set; }
        public ICommand OpenSteuernCommand { get; set; }
        public string Bestand
        {
            get => bestand;
            set
            {
                if (!double.TryParse(value, out double Bestand)) return;
                bestand = value;
                if (RequestIsWorking || Data.Bestand != Bestand)
                {
                    ValidateBestand(Bestand);
                    Data.Bestand = Bestand;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                    BerechneGesamtWerte();
                }
            }
        }
        public string DividendeText { get; private set; }
        public string Wechselkurs
        {
            get => wechselkurs;
            set
            {
                if (!double.TryParse(value, out double Wechselkurs)) return;
                wechselkurs = value;
                if (RequestIsWorking || Data.Umrechnungskurs != Wechselkurs)
                {
                    Data.Umrechnungskurs = Wechselkurs;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(WechsellkursHasValue));
                    ((DelegateCommand)OpenDividendeCommand).RaiseCanExecuteChanged();
                    BerechneGesamtWerte();
                }
            }
        }

        public static IEnumerable<DividendenRundungTypes> RundungTypes => Enum.GetValues(typeof(DividendenRundungTypes)).Cast<DividendenRundungTypes>();

        public DividendenRundungTypes RundungTyp
        {
            get { return Data.RundungArt; }
            set
            {
                if (RequestIsWorking || (Data.RundungArt != value))
                {
                    Data.RundungArt = value;
                    OnPropertyChanged();
                    BerechneGesamtWerte();
                }
            }
        }

        public double? Bemessungsgrundlage => Data.Bemessungsgrundlage;
        public double? Erhalten => Data.Erhalten;
        public double? ErhaltenUmgerechnetUngerundet => Data.ErhaltenUmgerechnetErmittelt.GetValueOrDefault(0);
        public double? SteuerVorZwischensumme => Data.SteuernVorZwischensumme.GetValueOrDefault(0);
        public double? SteuerNachZwischensumme => Data.SteuernNachZwischensumme.GetValueOrDefault(0);
        public double? Zwischensumme => Data.Zwischensumme.GetValueOrDefault(0);
        public double? ZwischensummeUmgerechnet => Data.ZwischensummeUmgerechnet.GetValueOrDefault(0);
        public string ZwischensummeTxt 
        { 
            get 
            {
                if (WechsellkursHasValue)
                    return Data.Zwischensumme.GetValueOrDefault(0).ToString("N2") + " / " + Data.ZwischensummeUmgerechnet.GetValueOrDefault(0).ToString("N2");
                else
                    return Data.Zwischensumme.GetValueOrDefault(0).ToString("N2");
            } 
        }

        public bool WechsellkursHasValue => Data.Umrechnungskurs.GetValueOrDefault(0) > 0;

        
        #endregion

        #region Commands
        private void ExecuteOpenAuswahlCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenDividendenAuswahlMessage(OpenDividendenAuswahlMessageCallback, Data.WertpapierID), "DividendeErhalten");
        }

        private void ExecuteOpenSteuernCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenSteuernUebersichtMessage(OpenSteuernUebersichtMessageCallback, Data.Steuer.Steuern), "DividendeErhalten");
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+ $"/api/DividendeErhalten?state={state}", Data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                     WeakReferenceMessenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp().ToString());
                     WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());
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
            return (Data.DividendeID != -1) && (Data.Umrechnungskurs.GetValueOrDefault(0)>0);
        }
        private void ExecuteOpenDividendeCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenDividendeProStueckAnpassenMessage { DividendeID = Data.DividendeID,  Umrechnungskurs = Data.Umrechnungskurs.Value });
        }

        #endregion

        #region Callbacks
        private void OpenDividendenAuswahlMessageCallback(bool confirmed, int id, double betrag, DateTime date)
        {
            if (confirmed)
            {
                DividendeAusgewaehlt(id, betrag, date);
            }
        }
        private void OpenSteuernUebersichtMessageCallback(bool confirmed, IList<SteuerModel> steuern)
        {
            if (confirmed)
            {
                Data.Steuer.Steuern = steuern;
                BerechneGesamtWerte();
            }
        }
        #endregion

        #region Validate
        private bool ValidateBestand(double bestand)
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

        protected override void OnActivated()
        {
            betrag = 0;
            Data = new DividendeErhaltenStammdatenModel { Steuer = new SteuergruppeModel { SteuerHerkunftTyp = SteuerHerkunftTyp.shtDividende, Steuern = [] } };
            DividendeText = "";
            DividendeID = -1;
            Bestand = "";
            wechselkurs = "";
            state = State.Neu;
            Wechselkurs = null;
            RundungTyp = DividendenRundungTypes.Normal;
        }

       
    }
}

