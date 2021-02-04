using Aktien.Data.Model.WertpapierModels;
using Aktien.Data.Types;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.ETFViewModels
{
    public class ETFStammdatenViewModel : ViewModelStammdaten
    {
        private Wertpapier etf;

        public ETFStammdatenViewModel() : base()
        {
            etf = new Wertpapier();
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);

            ISIN = "";
            Name = "";
            WKN = "";

            state = State.Neu;
        }


        protected override void ExecuteSaveCommand()
        {
            var api = new EtfAPI();
            if (state.Equals(State.Neu))
            {
                try
                {
                    api.Speichern(new Wertpapier() { ISIN = etf.ISIN, Name = etf.Name, WKN = etf.WKN });
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "ETF gespeichert." }, "ETFStammdaten");
                }
                catch (ETFSchonVorhandenException)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = false, Message = "ETF ist schon vorhanden." }, "ETFStammdaten");
                }
            }
            else
            {
                api.Aktualisieren(etf);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "ETF aktualisiert." }, "ETFStammdaten");
            }
        }

        public void Bearbeiten(int inID)
        {
            LoadAktie = true;
            var Loadaktie = new EtfAPI().LadeAnhandID(inID);
            etf = new Wertpapier
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
            get { return this.etf.ISIN; }
            set
            {

                if (LoadAktie || !string.Equals(this.etf.ISIN, value))
                {
                    ValidateISIN(value);
                    this.etf.ISIN = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string Name
        {
            get { return this.etf.Name; }
            set
            {
                if (LoadAktie || !string.Equals(this.etf.Name, value))
                {
                    ValidateName(value);
                    this.etf.Name = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string WKN
        {
            get { return this.etf.WKN; }
            set
            {

                if (LoadAktie || !string.Equals(this.etf.WKN, value))
                {
                    this.etf.WKN = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Validate
        private bool ValidateName(String inName)
        {
            var Validierung = new WertpapierStammdatenValidierung();

            bool isValid = Validierung.ValidateName(inName, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Name", validationErrors);
            return isValid;
        }

        private bool ValidateISIN(String inISIN)
        {
            var Validierung = new WertpapierStammdatenValidierung();

            bool isValid = Validierung.ValidateISIN(inISIN, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "ISIN", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            state = State.Neu;
            etf = new Wertpapier();
            ISIN = "";
            Name = "";
            WKN = "";
        }
    }
}
