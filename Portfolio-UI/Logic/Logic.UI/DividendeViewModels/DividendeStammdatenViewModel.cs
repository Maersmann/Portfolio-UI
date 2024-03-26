using Aktien.Data.Types.WertpapierTypes;
using CommunityToolkit.Mvvm.Messaging;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Base.Logic.ViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aktien.Data.Types;
using Aktien.Logic.Core.Validierung.Base;
using Data.Model.DividendeModels;
using System.Net;
using Aktien.Logic.Core;
using System.Net.Http;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;
using Base.Logic.Wrapper;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeStammdatenViewModel : ViewModelStammdaten<DividendeModel, StammdatenTypes>
    {
        private string betrag;
        private string betragUmgerechnet;
        public DividendeStammdatenViewModel()
        {
            Title = "Informationen Dividende";
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+"/api/dividende", Data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                     WeakReferenceMessenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp().ToString());
                     WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());
                }
                else if (resp.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    SendExceptionMessage("Fehler - Dividende");
                    return;
                }
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.dividende;

        #region Bindings
        public DateTime? Exdatum
        {
            get => Data.Exdatum;
            set
            {
                if (RequestIsWorking || !DateTime.Equals(Data.Exdatum, value))
                {
                    Data.Exdatum = value;
                    OnPropertyChanged();
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public DateTime? Zahldatum
        {
            get => Data.Zahldatum.Equals(DateTime.MinValue) ? null : (DateTime?)Data.Zahldatum;
            set
            {
                if (RequestIsWorking || !DateTime.Equals(Data.Zahldatum, value))
                {
                    ValidateDatum(value);
                    Data.Zahldatum = value.GetValueOrDefault();
                    OnPropertyChanged();
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Betrag
        {
            get => betrag;
            set
            {
                if (!double.TryParse(value, out double Betrag))
                {
                    ValidateBetrag(0);
                    betrag = "";
                    Data.Betrag = 0;
                    OnPropertyChanged();
                    return;
                }
                betrag = value;
                if (RequestIsWorking || Data.Betrag != Betrag)
                {
                    ValidateBetrag(Betrag);
                    Data.Betrag = Betrag;
                    OnPropertyChanged(); 
                }
            }
        }
        public string BetragUmgerechnet
        {
            get => betragUmgerechnet;
            set
            {
                if (!double.TryParse(value, out double BetragUmgerechnet))
                {
                    betragUmgerechnet = "";
                    Data.BetragUmgerechnet = 0;
                    OnPropertyChanged();
                    return;
                }
                if (RequestIsWorking || Data.BetragUmgerechnet != BetragUmgerechnet)
                {
                    Data.BetragUmgerechnet = BetragUmgerechnet;
                    OnPropertyChanged();
                }
            }
        }
        public Waehrungen Waehrung
        {
            get => Data.Waehrung;
            set
            {
                if (RequestIsWorking || (Data.Waehrung != value))
                {
                    Data.Waehrung = value;
                    OnPropertyChanged();
                }
            }
        }
        public static IEnumerable<Waehrungen> Waehrungen => Enum.GetValues(typeof(Waehrungen)).Cast<Waehrungen>();

        #endregion
        public int WertpapierID
        {
            set => Data.WertpapierID = value;
        }
        public async void Bearbeiten(int id)
        {
            RequestIsWorking = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/dividende/{id}");
                if (resp.IsSuccessStatusCode)
                {
                    Response = await resp.Content.ReadAsAsync<Response<DividendeModel>>();
                    WertpapierID = Data.WertpapierID;
                    Exdatum = Data.Exdatum;
                    Zahldatum = Data.Zahldatum;
                    Betrag = Data.Betrag.ToString();
                    Waehrung = Data.Waehrung;
                    BetragUmgerechnet = Data.BetragUmgerechnet.HasValue ? Data.BetragUmgerechnet.Value.ToString() : "";

                    state = State.Bearbeiten;
                }
            }
            RequestIsWorking = false;
        }

        #region Validate
        private bool ValidateDatum(DateTime? datun)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateDatum(datun, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Zahldatum", validationErrors);
            return isValid;
        }

        private bool ValidateBetrag(Double? betrag)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateBetrag(betrag, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Betrag", validationErrors);
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            return isValid;
        }
        #endregion

        protected override void OnActivated()
        {
            Data = new DividendeUebersichtModel();
            Zahldatum = DateTime.Now;
            Exdatum = null;
            state = State.Neu;
            Betrag = null;
            Waehrung = Aktien.Data.Types.WertpapierTypes.Waehrungen.Euro;
            OnPropertyChanged();
        }

    }
}
