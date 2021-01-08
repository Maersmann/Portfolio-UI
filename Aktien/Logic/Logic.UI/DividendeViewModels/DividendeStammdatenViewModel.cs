using Aktien.Data.Types;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Core.DividendeLogic;
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
using Aktien.Data.Model.AktieModels;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeStammdatenViewModel : ViewModelStammdaten
    {

        private Dividende dividende;

        public DividendeStammdatenViewModel()
        {
            Title = "Neue Dividende";
            dividende = new Dividende();
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Datum = DateTime.Now;
            state = State.Neu;
            Betrag = null;
        }

        protected override void ExecuteCloseCommand()
        {
            ViewModelLocator.CleanUpDividendeStammdatenView();
        }
        protected override void ExecuteSaveCommand()
        {
            var API = new DividendeAPI();
            if (state == State.Neu)
            {           
                API.SpeicherNeueDividende(dividende.Betrag, dividende.Datum, dividende.AktieID);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende gespeichert." });
            }
            else
            {
                API.UpdateDividende(dividende.Betrag, dividende.Datum, dividende.ID);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende aktualisiert." });
            }
        }

        public DateTime? Datum
        {
            get
            {
                return dividende.Datum;
            }
            set
            {
                if (ValidateDatum(value) || !DateTime.Equals(this.dividende.Datum, value))
                {
                    this.dividende.Datum = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public Double? Betrag
        {
            get
            {
                return dividende.Betrag;
            }
            set
            {
                if (ValidateBetrag(value) || (this.dividende.Betrag != value))
                {
                    this.dividende.Betrag = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public String Aktienname
        {
            get
            {
                return dividende.Aktie.Name;
            }
            set
            {
                if ( (this.dividende.Aktie.Name != value))
                {
                    this.dividende.Aktie.Name = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public int AktieID
        {
            set { dividende.AktieID = value; }
        }
        public int ID
        {
            set
            {
                var _dividende = new DividendeAPI().LadeAnhandID(value);

                dividende = new Dividende
                {
                    ID = _dividende.ID
                };

                AktieID = _dividende.AktieID;
                Datum = _dividende.Datum;
                Betrag = _dividende.Betrag;
                state = State.Bearbeiten;
            }
        }


        #region Validate
        private bool ValidateDatum(DateTime? inDatum)
        {
            const string propertyKey = "Datum";
            var Validierung = new DividendeStammdatenValidierung();

            bool isValid = Validierung.ValidateDatum(inDatum, out ICollection<string> validationErrors);


            if (!isValid)
            {

                ValidationErrors[propertyKey] = validationErrors;

                RaiseErrorsChanged(propertyKey);
            }
            else if (ValidationErrors.ContainsKey(propertyKey))
            {

                ValidationErrors.Remove(propertyKey);

                RaiseErrorsChanged(propertyKey);
            }
            return isValid;
        }

        private bool ValidateBetrag(Double? inBetrag)
        {
            const string propertyKey = "Betrag";
            var Validierung = new DividendeStammdatenValidierung();

            bool isValid = Validierung.ValidateBetrag(inBetrag, out ICollection<string> validationErrors);


            if (!isValid)
            {

                ValidationErrors[propertyKey] = validationErrors;

                RaiseErrorsChanged(propertyKey);
            }
            else if (ValidationErrors.ContainsKey(propertyKey))
            {

                ValidationErrors.Remove(propertyKey);

                RaiseErrorsChanged(propertyKey);
            }
            return isValid;
        }
        #endregion
    }
}
