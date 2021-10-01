using Aktien.Data.Types.WertpapierTypes;
using GalaSoft.MvvmLight.Messaging;
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

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeStammdatenViewModel : ViewModelStammdaten<DividendeModel, StammdatenTypes>
    {
        public DividendeStammdatenViewModel()
        {
            Title = "Informationen Dividende";
            Cleanup();
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+"/api/dividende", data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
                    Messenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());
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
            get => data.Exdatum;
            set
            {
                if (RequestIsWorking || !DateTime.Equals(data.Exdatum, value))
                {
                    data.Exdatum = value;
                    RaisePropertyChanged();
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public DateTime? Zahldatum
        {
            get => data.Zahldatum.Equals(DateTime.MinValue) ? null : (DateTime?)data.Zahldatum;
            set
            {
                if (RequestIsWorking || !DateTime.Equals(data.Zahldatum, value))
                {
                    ValidateDatum(value);
                    data.Zahldatum = value.GetValueOrDefault();
                    RaisePropertyChanged();
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public double? Betrag
        {
            get => data.Betrag;
            set
            {
                if (RequestIsWorking || data.Betrag != value)
                {
                    ValidateBetrag(value);
                    data.Betrag = value.GetValueOrDefault();
                    RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public double? BetragUmgerechnet
        {
            get => data.BetragUmgerechnet;
            set
            {
                if (RequestIsWorking || data.BetragUmgerechnet != value)
                {
                    data.BetragUmgerechnet = value;
                    RaisePropertyChanged();
                }
            }
        }
        public Waehrungen Waehrung
        {
            get => data.Waehrung;
            set
            {
                if (RequestIsWorking || (data.Waehrung != value))
                {
                    data.Waehrung = value;
                    RaisePropertyChanged();
                }
            }
        }
        public static IEnumerable<Waehrungen> Waehrungen => Enum.GetValues(typeof(Waehrungen)).Cast<Waehrungen>();

        #endregion
        public int WertpapierID
        {
            set => data.WertpapierID = value;
        }
        public async void Bearbeiten(int id)
        {
            RequestIsWorking = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/dividende/{id}");
                if (resp.IsSuccessStatusCode)
                { 
                    data = await resp.Content.ReadAsAsync<DividendeModel>();
                    WertpapierID = data.WertpapierID;
                    Exdatum = data.Exdatum;
                    Zahldatum = data.Zahldatum;
                    Betrag = data.Betrag;
                    Waehrung = data.Waehrung;
                    BetragUmgerechnet = data.BetragUmgerechnet;
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
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            data = new DividendeUebersichtModel();
            Zahldatum = DateTime.Now;
            Exdatum = null;
            state = State.Neu;
            Betrag = null;
            Waehrung = Data.Types.WertpapierTypes.Waehrungen.Euro;
            this.RaisePropertyChanged();
        }

    }
}
