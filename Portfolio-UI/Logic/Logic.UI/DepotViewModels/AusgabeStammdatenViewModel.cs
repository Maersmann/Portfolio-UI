using Aktien.Data.Types;
using Aktien.Data.Types.DepotTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using Aktien.Logic.UI.InterfaceViewModels;
using Data.Model.DepotModels;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class AusgabeStammdatenViewModel : ViewModelStammdaten<AusgabeModel>, IViewModelStammdaten
    {
        public AusgabeStammdatenViewModel()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Cleanup();
        }

        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.ausgaben;

        public int DepotID { set { data.DepotID = value; } }

        #region Bindings
        public IEnumerable<AusgabenArtTypes> AusgabeTypes
        {
            get
            {
                return Enum.GetValues(typeof(AusgabenArtTypes)).Cast<AusgabenArtTypes>();
            }
        }
        public AusgabenArtTypes AusgabeTyp
        {
            get
            {
                return data.Art;
            }
            set
            {
                if ((this.data.Art != value))
                {
                    data.Art = value;
                    this.RaisePropertyChanged();
                }
            }

        }
        public DateTime? Datum
        {
            get
            {
                return data.Datum;
            }
            set
            {
                if (!DateTime.Equals(this.data.Datum, value))
                {
                    ValidateDatum(value);
                    this.data.Datum = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public double? Betrag
        {
            get
            {
                return data.Betrag;
            }
            set
            {
                if (LoadAktie || (this.data.Betrag != value))
                {
                    ValidateBetrag(value);
                    this.data.Betrag = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public String Beschreibung
        {
            get
            {
                return data.Beschreibung;
            }
            set
            {
                if (this.data.Beschreibung != value)
                {
                    this.data.Beschreibung = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Commands
        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+"/api/depot/Ausgabe", data);

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Ausgabe gespeichert." }, GetStammdatenTyp());
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.ausgaben);
                }
                else
                {
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                    return;
                }
            }
        }

        #endregion

        #region Validate
        private bool ValidateBetrag(Double? betrag)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateBetrag(betrag, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Betrag", validationErrors);
            return isValid;
        }

        private bool ValidateDatum(DateTime? datum)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateDatum(datum, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Datum", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            state = State.Neu;
            data = new AusgabeModel();
            Betrag = null;
            Datum = DateTime.Now;
            DepotID = 1;
            Beschreibung = "";
        }

        public void ZeigeStammdatenAn(int id)
        {
            throw new NotImplementedException();
        }
    }
}
