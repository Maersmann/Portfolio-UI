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

        private Aktie aktie;

        private bool LoadAktie;

        public AktieStammdatenViewModel():base()
        {
            aktie = new Aktie();
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);

            ISIN = "";
            Name = "";
            WKN = "";

            state = State.Neu;

            LoadAktie = false;
        }


        protected override void ExecuteSaveCommand()
        {
            var api = new AktieAPI();
            if (state.Equals( State.Neu ))
            {            
                try
                { 
                    api.Speichern(new Aktie() { ISIN = aktie.ISIN, Name = aktie.Name, WKN = aktie.WKN });
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Aktie erfolgreich gespeichert." });
                }
                catch( AktieSchonVorhandenException)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = false, Message = "Aktie ist schon vorhanden." });
                }
            }
            else
            {
                api.Update(aktie);
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
                LoadAktie = true;
                aktie = new AktieAPI().LadeAnhandID(value);
                WKN = aktie.WKN;
                Name = aktie.Name;
                ISIN = aktie.ISIN;
                LoadAktie = false;
                state = State.Bearbeiten;
                this.RaisePropertyChanged("ISIN_isEnabled");
                

            } 
        }


        public bool ISIN_isEnabled { get{ return state == State.Neu; } }
        public string ISIN
        {
            get { return this.aktie.ISIN; }
            set
            {

                if (LoadAktie || !string.Equals(this.aktie.ISIN, value))
                {
                    ValidateISIN(value);
                    this.aktie.ISIN = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string Name{
            get { return this.aktie.Name; }
            set
            {               
                if (LoadAktie || !string.Equals(this.aktie.Name, value))
                {
                    ValidateName(value);
                    this.aktie.Name = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string WKN
        {
            get { return this.aktie.WKN; }
            set
            {

                if (LoadAktie || !string.Equals(this.aktie.WKN, value))
                {
                    this.aktie.WKN = value;
                    this.RaisePropertyChanged();
                }
            }
        }


        #region Validate
        private bool ValidateName(String inName)
        {
            var Validierung = new AktieStammdatenValidierung();

            bool isValid = Validierung.ValidateName(inName, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Name", validationErrors);
            return isValid;
        }

        private bool ValidateISIN(String inISIN)
        {
            var Validierung = new AktieStammdatenValidierung();

            bool isValid = Validierung.ValidateISIN(inISIN, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "ISIN", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            state = State.Neu;
            aktie = new Aktie();
            ISIN = "";
            Name = "";
            WKN = "";
        }

    }
}
