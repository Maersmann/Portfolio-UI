using Aktien.Data.Types;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.Base;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Logic.UI.DividendeViewModels
{
    public class DividendeReitAktualisierungViewModel : ViewModelStammdaten<DividendeReitAktualisierungModel, StammdatenTypes>
    {
        private double ursprungsBetrag;
        private double stornierterBetrag;
        private double aktualisierterBetrag;
        private DividendeErhaltenModel dividendeErhalten;
        public DividendeReitAktualisierungViewModel()
        {
            Title = "REIT Aktualisierung";
            OpenSteuernCommand = new RelayCommand(ExecuteOpenSteuernCommand);
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + "/api/dividendeErhalten/Reit/Aktualisierung", Data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                     WeakReferenceMessenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp().ToString());
                     WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());
                }
                else if (resp.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    SendExceptionMessage("Fehler - Dividende Reit Aktualisierung");
                    return;
                }
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.dividendeErhalten;

        #region Bindings

        public double UrsprungsBetrag
        {
            get => ursprungsBetrag;
            set
            {
                ursprungsBetrag = value;
                OnPropertyChanged();
            }
        }

        public double StornierterBetrag
        {
            get => stornierterBetrag;
            set
            {
                stornierterBetrag = value;
                OnPropertyChanged();
            }
        }

        public double AktualisierterBetrag
        {
            get => aktualisierterBetrag;
            set
            {
                aktualisierterBetrag = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<DividendenRundungTypes> RundungTypes => Enum.GetValues(typeof(DividendenRundungTypes)).Cast<DividendenRundungTypes>();

        public DividendenRundungTypes RundungArtAktualisierung
        {
            get { return Data.RundungArtAktualisierung; }
            set
            {
                if (RequestIsWorking || (Data.RundungArtAktualisierung != value))
                {
                    Data.RundungArtAktualisierung = value;
                    OnPropertyChanged();
                    BerechneGesamtWerte();
                }
            }
        }

        public DividendenRundungTypes RundungArtStornierung
        {
            get { return Data.RundungArtStornierung; }
            set
            {
                if (RequestIsWorking || (Data.RundungArtStornierung != value))
                {
                    Data.RundungArtStornierung = value;
                    OnPropertyChanged();
                    BerechneGesamtWerte();
                }
            }
        }
        public ICommand OpenSteuernCommand { get; set; }
        #endregion

        public async void LoadingDividendeErhalten(int id)
        {
            Data.DividendeErhaltenID = id;
            RequestIsWorking = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/dividendeErhalten/{id}");
                if (resp.IsSuccessStatusCode)
                {
                    Response<DividendeErhaltenModel> DividendeErhaltenResponse = await resp.Content.ReadAsAsync<Response<DividendeErhaltenModel>>();
                    DividendeErhaltenResponse.Data.Steuer.Steuern.ToList().ForEach(steuer =>
                    {
                        if (steuer.Steuerart.BerechnungZwischensumme.Equals(SteuerberechnungZwischensumme.vorZwischensumme))
                        {
                            Data.SteuernStorniert.Add(steuer);
                        }
                    });
                    dividendeErhalten = DividendeErhaltenResponse.Data;
                    RundungArtStornierung = dividendeErhalten.RundungArt;
                    RundungArtAktualisierung = dividendeErhalten.RundungArt;

                    BerechneGesamtWerte();
                }
            }
            RequestIsWorking = false;
        }

        private void ExecuteOpenSteuernCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenSteuernUebersichtMessage(OpenSteuernUebersichtMessageCallback, Data.SteuernNeu), "DividendeErhaltenReit");
        }

        private void OpenSteuernUebersichtMessageCallback(bool confirmed, IList<SteuerModel> steuern)
        {
            if (confirmed)
            {
                Data.SteuernNeu = steuern;
                BerechneGesamtWerte();
            }
        }

        public void BerechneGesamtWerte()
        {
            UrsprungsBetrag = dividendeErhalten.Erhalten;

            double steuerVorZwischenSumme = 0;
            double steuerNachZwischenSumme = 0;

            if (Data.SteuernStorniert != null)
            {
                steuerVorZwischenSumme = new SteuerBerechnen().SteuernVorZwischensumme(Data.SteuernStorniert);
                steuerNachZwischenSumme = new SteuerBerechnen().SteuernNachZwischensumme(Data.SteuernStorniert);
            }
            double zwischensumme = dividendeErhalten.Bemessungsgrundlage + steuerVorZwischenSumme;
            double erhalten = zwischensumme + steuerNachZwischenSumme;
            if (dividendeErhalten.Umrechnungskurs.HasValue)
            {
                zwischensumme = new DividendenBerechnungen().BetragUmgerechnet(zwischensumme, dividendeErhalten.Umrechnungskurs, true, Data.RundungArtStornierung);
                erhalten = zwischensumme + steuerNachZwischenSumme;
            }

            StornierterBetrag = erhalten;

            if (Data.SteuernNeu != null)
            {
                steuerVorZwischenSumme = new SteuerBerechnen().SteuernVorZwischensumme(Data.SteuernNeu);
                steuerNachZwischenSumme = new SteuerBerechnen().SteuernNachZwischensumme(Data.SteuernNeu);
            }
            zwischensumme = dividendeErhalten.Bemessungsgrundlage + steuerVorZwischenSumme;
            erhalten = zwischensumme + steuerNachZwischenSumme;

            if (dividendeErhalten.Umrechnungskurs.HasValue)
            {
                zwischensumme = new DividendenBerechnungen().BetragUmgerechnet(zwischensumme, dividendeErhalten.Umrechnungskurs, true, Data.RundungArtAktualisierung);
                erhalten = zwischensumme + steuerNachZwischenSumme;
            }

            AktualisierterBetrag = erhalten;

        }


        protected override void OnActivated()
        {
            Data = new DividendeReitAktualisierungModel();
            state = State.Neu;
            OnPropertyChanged();
            UrsprungsBetrag = 0;
            AktualisierterBetrag = 0;
            StornierterBetrag = 0;
        }

    }
}
