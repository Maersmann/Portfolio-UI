using Aktien.Data.Types.WertpapierTypes;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.BaseViewModels;
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

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeStammdatenViewModel : ViewModelStammdaten<DividendeModel>
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
                HttpResponseMessage resp = await Client.PostAsJsonAsync("https://localhost:5001/api/dividende", data);

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
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.dividende;

        #region Bindings
        public DateTime? Exdatum
        {
            get
            {
                return data.Exdatum;
            }
            set
            {
                if (LoadAktie || !DateTime.Equals(this.data.Exdatum, value))
                {
                    this.data.Exdatum = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public DateTime? Zahldatum
        {
            get
            {
                if (data.Zahldatum.Equals(DateTime.MinValue))
                    return null;
                else
                    return data.Zahldatum;
            }
            set
            {
                if (LoadAktie ||!DateTime.Equals(this.data.Zahldatum, value))
                {
                    ValidateDatum(value);
                    this.data.Zahldatum = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public Double? Betrag
        {
            get
            {
                return data.Betrag;
            }
            set
            {
                if (LoadAktie || this.data.Betrag != value)
                {
                    ValidateBetrag(value);
                    this.data.Betrag = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public Double? BetragUmgerechnet
        {
            get
            {
                return data.BetragUmgerechnet;
            }
            set
            {
                if (LoadAktie || this.data.BetragUmgerechnet != value)
                {
                    this.data.BetragUmgerechnet = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public Waehrungen Waehrung
        {
            get { return data.Waehrung; }
            set
            {
                if (LoadAktie || (this.data.Waehrung != value))
                {
                    this.data.Waehrung = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public static IEnumerable<Waehrungen> Waehrungen
        {
            get
            {
                return Enum.GetValues(typeof(Waehrungen)).Cast<Waehrungen>();
            }
        }

        #endregion
        public int WertpapierID
        {
            set { data.WertpapierID = value; }
        }
        public async void Bearbeiten(int id)
        {
            LoadAktie = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync($"https://localhost:5001/api/dividende/{id}");
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
            LoadAktie = false;
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
