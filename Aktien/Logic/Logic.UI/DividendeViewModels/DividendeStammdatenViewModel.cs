﻿using Aktien.Data.Types;
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
            dividende = new Dividende();
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Datum = DateTime.Now;
            state = State.Neu;
            Betrag = null;
        }

        protected override void ExecuteSaveCommand()
        {
            var API = new DividendeAPI();
            if (state == State.Neu)
            {           
                API.Speichern(dividende.Betrag, dividende.Datum, dividende.AktieID);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende gespeichert." });
            }
            else
            {
                API.Aktualisiere(dividende.Betrag, dividende.Datum, dividende.ID);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende aktualisiert." });
            }
        }

        #region Bindings
        public DateTime? Datum
        {
            get
            {
                return dividende.Datum;
            }
            set
            {
                if ( !DateTime.Equals(this.dividende.Datum, value))
                {
                    ValidateDatum(value);
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
                if (this.dividende.Betrag != value)
                {
                    ValidateBetrag(value);
                    this.dividende.Betrag = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        #endregion
        public int AktieID
        {
            set { dividende.AktieID = value; }
        }
        public void ZeigeDividiende(int inID)
        {
            var _dividende = new DividendeAPI().LadeAnhandID(inID);

            dividende = new Dividende
            {
                ID = _dividende.ID
             };

             AktieID = _dividende.AktieID;
             Datum = _dividende.Datum;
             Betrag = _dividende.Betrag;
             state = State.Bearbeiten;
        }


        #region Validate
        private bool ValidateDatum(DateTime? inDatum)
        {
            var Validierung = new DividendeStammdatenValidierung();

            bool isValid = Validierung.ValidateDatum(inDatum, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Datum", validationErrors);
            return isValid;
        }

        private bool ValidateBetrag(Double? inBetrag)
        {
            var Validierung = new DividendeStammdatenValidierung();

            bool isValid = Validierung.ValidateBetrag(inBetrag, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Betrag", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            state = State.Neu;
            dividende = new Dividende();
            this.RaisePropertyChanged();
            Datum = DateTime.Now;
            Betrag = null;
        }

    }
}
