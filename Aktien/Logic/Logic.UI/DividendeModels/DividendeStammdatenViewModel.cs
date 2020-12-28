﻿using Data.API;
using GalaSoft.MvvmLight.Messaging;
using Logic.Core.Validierung;
using Logic.Messages.Base;
using Logic.Messages.DividendeMessages;
using Logic.Models.DividendeModels;
using Logic.UI.BaseModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.UI.DividendeModels
{
    public class DividendeStammdatenViewModel : ViewModelStammdatan
    {

        private readonly Dividende dividende;

        public DividendeStammdatenViewModel()
        {
            Title = "Neue Dividende";
            dividende = new Dividende();
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Datum = DateTime.Now;
            Betrag = null;
        }

        protected override void ExecuteCloseCommand()
        {
            ViewModelLocator.CleanUpDividendeStammdatenView();
        }

        protected override void ExecuteSaveCommand()
        {
            var API = new DividendeAPI();
            API.SpeicherNeueDividende(dividende.Betrag.GetValueOrDefault(), dividende.Datum.GetValueOrDefault(), dividende.AktienID.GetValueOrDefault());
            Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende erfolgreich gespeichert." });
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
                    this.dividende.Datum = value;
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
                    this.dividende.Betrag = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public String Aktienname
        {
            get
            {
                return dividende.Aktienname;
            }
            set
            {
                if ( (this.dividende.Aktienname != value))
                {
                    this.dividende.Aktienname = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public int AktienID
        {
            set { dividende.AktienID = value; }
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