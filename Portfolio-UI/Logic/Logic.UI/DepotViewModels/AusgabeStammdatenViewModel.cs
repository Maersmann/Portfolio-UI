using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Base.Logic.ViewModels;
using Aktien.Logic.UI.InterfaceViewModels;
using Data.Model.DepotModels;
using CommunityToolkit.Mvvm.Messaging;
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
using Data.Types.DepotTypes;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class AusgabeStammdatenViewModel : ViewModelStammdaten<AusgabeModel, StammdatenTypes>, IViewModelStammdaten
    {
        private string betrag;
        public AusgabeStammdatenViewModel()
        {
            Title = "Neue Ausgabe eintragen";
        }

        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.ausgaben;

        public int DepotID { set => Data.DepotID = value; }

        #region Bindings
        public IEnumerable<AusgabenArtTypes> AusgabeTypes => Enum.GetValues(typeof(AusgabenArtTypes)).Cast<AusgabenArtTypes>();
        public AusgabenArtTypes AusgabeTyp
        {
            get => Data.Art;
            set
            {
                if ((Data.Art != value))
                {
                    Data.Art = value;
                    OnPropertyChanged();
                }
            }

        }
        public DateTime? Datum
        {
            get => Data.Datum;
            set
            {
                if (!Equals(Data.Datum, value))
                {
                    ValidateDatum(value);
                    Data.Datum = value.GetValueOrDefault();
                    OnPropertyChanged();
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
                    Data.Betrag = 0;
                    OnPropertyChanged();
                    return;
                }
                betrag = value;
                if (RequestIsWorking || (Data.Betrag != Betrag))
                {
                    ValidateBetrag(Betrag);
                    Data.Betrag = Betrag;
                    OnPropertyChanged();
                }
            }
        }
        public string Beschreibung
        {
            get => Data.Beschreibung;
            set
            {
                if (Data.Beschreibung != value)
                {
                    Data.Beschreibung = value;
                    OnPropertyChanged();
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
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+"/api/depot/Ausgabe", Data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                     WeakReferenceMessenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Ausgabe gespeichert." }, GetStammdatenTyp().ToString());
                     WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage(), StammdatenTypes.ausgaben.ToString());
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

        protected override void OnActivated()
        {
            state = State.Neu;
            Data = new AusgabeModel();
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
