using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.UI.InterfaceViewModels;

namespace Aktien.Logic.UI.AktieViewModels
{
    public class AktieStammdatenViewModel : ViewModelStammdaten, IViewModelStammdaten
    {

        private Wertpapier aktie;

        public AktieStammdatenViewModel():base()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Cleanup();
        }


        protected override void ExecuteSaveCommand()
        {
            var api = new AktieAPI();
            if (state.Equals( State.Neu ))
            {            
                try
                { 
                    api.Speichern(new Wertpapier() { ISIN = aktie.ISIN, Name = aktie.Name, WKN = aktie.WKN });
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Aktie gespeichert." }, "AktieStammdaten");
                }
                catch( WertpapierSchonVorhandenException)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = false, Message = "Aktie ist schon vorhanden." }, "AktieStammdaten");
                    return;
                }
            }
            else
            {
                api.Aktualisieren(aktie);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Aktie aktualisiert." }, "AktieStammdaten");
            }
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.aktien);
        }


        public void ZeigeStammdatenAn(int id) 
        { 
            LoadAktie = true;
            var Loadaktie = new AktieAPI().LadeAnhandID(id);
            aktie = new Wertpapier
            {
                ID = Loadaktie.ID
            };

            WKN = Loadaktie.WKN;
            Name = Loadaktie.Name;
            ISIN = Loadaktie.ISIN;
            LoadAktie = false;
            state = State.Bearbeiten;
            this.RaisePropertyChanged("ISIN_isEnabled");
             

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
        private bool ValidateName(String name)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(name, "Name", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Name", validationErrors);
            return isValid;
        }

        private bool ValidateISIN(String isin)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(isin, "ISIN", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "ISIN", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            state = State.Neu;
            aktie = new Wertpapier();
            ISIN = "";
            Name = "";
            WKN = "";
        }

    }
}
