using Aktien.Data.Types;
using Aktien.Data.Types.DepotTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Base.Logic.ViewModels;
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
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class AusgabeStammdatenViewModel : ViewModelStammdaten<AusgabeModel, StammdatenTypes>, IViewModelStammdaten
    {
        private string betrag;
        public AusgabeStammdatenViewModel()
        {
            SaveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            Cleanup();
        }

        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.ausgaben;

        public int DepotID { set => data.DepotID = value; }

        #region Bindings
        public IEnumerable<AusgabenArtTypes> AusgabeTypes => Enum.GetValues(typeof(AusgabenArtTypes)).Cast<AusgabenArtTypes>();
        public AusgabenArtTypes AusgabeTyp
        {
            get => data.Art;
            set
            {
                if ((data.Art != value))
                {
                    data.Art = value;
                    RaisePropertyChanged();
                }
            }

        }
        public DateTime? Datum
        {
            get => data.Datum;
            set
            {
                if (!Equals(data.Datum, value))
                {
                    ValidateDatum(value);
                    data.Datum = value.GetValueOrDefault();
                    RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
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
                    ValidateBetrag(Betrag);
                    betrag = "";
                    data.Betrag = 0;
                    RaisePropertyChanged();
                    return;
                }
                betrag = value;
                if (RequestIsWorking || (data.Betrag != Betrag))
                {
                    ValidateBetrag(Betrag);
                    data.Betrag = Betrag;
                    RaisePropertyChanged();
                }
            }
        }
        public string Beschreibung
        {
            get => data.Beschreibung;
            set
            {
                if (data.Beschreibung != value)
                {
                    data.Beschreibung = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Commands
        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+"/api/depot/Ausgabe", data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Ausgabe gespeichert." }, GetStammdatenTyp());
                    Messenger.Default.Send(new AktualisiereViewMessage(), StammdatenTypes.ausgaben.ToString());
                }
                else
                {
                    SendExceptionMessage("Ausgabe konnte nicht gespeichert werden.");
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
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
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
            Betrag = "";
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
