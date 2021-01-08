using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.Aktie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aktien.Logic.Core.Validierung;
using System.Runtime.CompilerServices;
using Prism.Commands;
using System.Windows.Input;
using Aktien.Logic.Messages.Base;
using Aktien.Data.Types;
using Aktien.Logic.UI.BaseViewModels;
using Aktien.Logic.Core.AktieLogic;
using Aktien.Data.Model.AktieModels;

namespace Aktien.Logic.UI.AktieViewModels
{
    public class AktieStammdatenViewModel : ViewModelStammdaten
    {

        private Aktie updateAktie;

        private string name;

        private string isin;

        private string wkn;

        public AktieStammdatenViewModel():base()
        {
            Title = "Neue Aktie";
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);

            ValidateISIN("");
            ValidateName("");

            state = State.Neu;
        }


        protected override void ExecuteSaveCommand()
        {
            var api = new AktieAPI();
            if (state.Equals( State.Neu ))
            {            
                try
                { 
                    api.Speichern(new Aktie() { ISIN = isin, Name = name, WKN = wkn });
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Aktie erfolgreich gespeichert." });
                }
                catch( AktieSchonVorhandenException)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = false, Message = "Aktie ist schon vorhanden." });
                }
            }
            else
            {
                updateAktie.Name = name;
                updateAktie.WKN = wkn;
                api.Update(updateAktie);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Aktie erfolgreich aktualisiert." });
            } 
        }

        protected override void ExecuteCloseCommand()
        {
            ViewModelLocator.CleanUpAktieStammdatenView();    
        }

        public int AktieID 
        { 
            set 
            { 
                Title = "Aktie bearbeiten";
                updateAktie = new AktieAPI().LadeAnhandID(value);
                Name = updateAktie.Name;
                ISIN = updateAktie.ISIN;
                WKN = updateAktie.WKN;
                state = State.Bearbeiten;
                this.RaisePropertyChanged("ISIN_isEnabled");
            } 
        }


        public bool ISIN_isEnabled { get{ return state == State.Neu; } }

        public string ISIN
        {
            get { return this.isin; }
            set
            {

                if (ValidateISIN(value) || !string.Equals(this.isin, value))
                {
                    this.isin = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Name{
            get { return this.name; }
            set
            {               
                if (ValidateName(value) || !string.Equals(this.name, value))
                {
                    this.name = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string WKN
        {
            get { return this.wkn; }
            set
            {

                if (!string.Equals(this.wkn, value))
                {
                    this.wkn = value;
                    this.RaisePropertyChanged();
                }
            }
        }


        #region Validate
        private bool ValidateName(String inName)
        {
            const string propertyKey = "Name";
            var Validierung = new AktieStammdatenValidierung();

            bool isValid = Validierung.ValidateName(inName, out ICollection<string> validationErrors);


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

        private bool ValidateISIN(String inISIN)
        {
            const string propertyKey = "ISIN";
            var Validierung = new AktieStammdatenValidierung();

            bool isValid = Validierung.ValidateISIN(inISIN, out ICollection<string> validationErrors);


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
