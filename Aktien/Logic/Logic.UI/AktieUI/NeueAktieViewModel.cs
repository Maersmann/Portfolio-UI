using Data;
using Data.API;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Core.Validierung;
using System.Runtime.CompilerServices;
using Prism.Commands;
using System.Windows.Input;
using Logic.UI.BaseModels;

namespace Logic.UI.AktieUI
{
    public class NeueAktieViewModel : ViewModelStammdatan
    {
        private string name;

        private string isin;

        private string wkn;

        public NeueAktieViewModel():base()
        {
            Title = "Neue Aktie";
            name = "";
            SaveNeueAktieCommand = new DelegateCommand<string>(this.ExecuteSaveNeueAktieCommand, this.CanExecuteSaveNeueAktieCommand);

            ValidateISIN("");
            ValidateName("");
        }

        protected override void ExecuteSaveNeueAktieCommand(String arg)
        {
            AktieAPI api = new AktieAPI();
            if (!api.IstAkieVorhanden( isin ))
            { 
                api.Speichern(new Aktie() { ISIN = isin, Name = name });
                Messenger.Default.Send<SaveNeueAktieResultMessage>(new SaveNeueAktieResultMessage { Erfolgreich = true });
            }
            else
            {
                Messenger.Default.Send<SaveNeueAktieResultMessage>(new SaveNeueAktieResultMessage { Erfolgreich = false, Fehlermessage = "Aktie ist schon vorhanden" });
            }
        }

        protected override void ExecuteOpenCommand()
        {
            ViewModelLocator.CleanUpNeueAktieView();
        }


        public string ISIN
        {
            get { return this.isin; }
            set
            {

                if (ValidateISIN(value) || !string.Equals(this.isin, value))
                {
                    this.isin = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand<string>)SaveNeueAktieCommand).RaiseCanExecuteChanged();
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
                    ((DelegateCommand<string>)SaveNeueAktieCommand).RaiseCanExecuteChanged();
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



        private bool ValidateName(String inName)
        {
            const string propertyKey = "Name";
            var Validierung = new NeueAktieValidierung();
            ICollection<string> validationErrors = null;

            bool isValid = Validierung.ValidateName(inName, out validationErrors);


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
            var Validierung = new NeueAktieValidierung();
            ICollection<string> validationErrors = null;

            bool isValid = Validierung.ValidateISIN(inISIN, out validationErrors);


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

    


    }
}
