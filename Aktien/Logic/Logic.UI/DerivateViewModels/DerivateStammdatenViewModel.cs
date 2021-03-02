using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using Aktien.Logic.UI.InterfaceViewModels;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.DerivateViewModels
{
    public class DerivateStammdatenViewModel : ViewModelStammdaten, IViewModelStammdaten
    {
        private Wertpapier derivate;

        public DerivateStammdatenViewModel() : base()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Cleanup();
        }


        protected override void ExecuteSaveCommand()
        {
            var api = new DerivateAPI();
            if (state.Equals(State.Neu))
            {
                try
                {
                    api.Speichern(new Wertpapier() { ISIN = derivate.ISIN, Name = derivate.Name, WKN = derivate.WKN });
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Derivate gespeichert." }, "DerivateStammdaten");
                }
                catch (WertpapierSchonVorhandenException)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = false, Message = "Derivate ist schon vorhanden." }, "DerivateStammdaten");
                    return;
                }
            }
            else
            {
                api.Aktualisieren(derivate);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Derivate aktualisiert." }, "DerivateStammdaten");
            }
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewWertpapierUebersicht);
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewDerivateUebersicht);
        }

        public void ZeigeStammdatenAn(int id)
        {
            LoadAktie = true;
            var Loadaktie = new DerivateAPI().LadeAnhandID(id);
            derivate = new Wertpapier
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


        public bool ISIN_isEnabled { get { return state == State.Neu; } }

        #region Bindings   
        public string ISIN
        {
            get { return this.derivate.ISIN; }
            set
            {

                if (LoadAktie || !string.Equals(this.derivate.ISIN, value))
                {
                    ValidateISIN(value);
                    this.derivate.ISIN = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string Name
        {
            get { return this.derivate.Name; }
            set
            {
                if (LoadAktie || !string.Equals(this.derivate.Name, value))
                {
                    ValidateName(value);
                    this.derivate.Name = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string WKN
        {
            get { return this.derivate.WKN; }
            set
            {

                if (LoadAktie || !string.Equals(this.derivate.WKN, value))
                {
                    this.derivate.WKN = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

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
            derivate = new Wertpapier();
            ISIN = "";
            Name = "";
            WKN = "";
        }
    }
}
