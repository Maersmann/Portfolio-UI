using Aktien.Data.Types.WertpapierTypes;
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
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeStammdatenViewModel : ViewModelStammdaten
    {

        private Dividende dividende;

        public DividendeStammdatenViewModel()
        {
            Title = "Informationen Dividende";
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Cleanup();
        }

        protected override void ExecuteSaveCommand()
        {
            var WertpapiedID = dividende.WertpapierID;
            var API = new DividendeAPI();
            if (state == State.Neu)
            {           
                API.Speichern(dividende.Betrag, dividende.Zahldatum, dividende.Exdatum, dividende.WertpapierID, dividende.Waehrung, dividende.BetragUmgerechnet, dividende.RundungArt);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende gespeichert." }, "DividendenStammdaten");
            }
            else
            {
                API.Aktualisiere(dividende.Betrag, dividende.Zahldatum, dividende.Exdatum, dividende.ID, dividende.Waehrung, dividende.BetragUmgerechnet, dividende.RundungArt);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende aktualisiert." }, "DividendenStammdaten");
            }
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage { ID =  WertpapiedID }, ViewType.viewDividendeUebersicht);
        }

        #region Bindings
        public DateTime? Exdatum
        {
            get
            {
                return dividende.Exdatum;
            }
            set
            {
                if ( !DateTime.Equals(this.dividende.Exdatum, value))
                {
                    this.dividende.Exdatum = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public DateTime? Zahldatum
        {
            get
            {
                if (dividende.Zahldatum.Equals(DateTime.MinValue))
                    return null;
                else
                    return dividende.Zahldatum;
            }
            set
            {
                if (LoadAktie ||!DateTime.Equals(this.dividende.Zahldatum, value))
                {
                    ValidateDatum(value);
                    this.dividende.Zahldatum = value.GetValueOrDefault();
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
        public Double? BetragUmgerechnet
        {
            get
            {
                return dividende.BetragUmgerechnet;
            }
            set
            {
                if (this.dividende.BetragUmgerechnet != value)
                {
                    this.dividende.BetragUmgerechnet = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public Waehrungen Waehrung
        {
            get { return dividende.Waehrung; }
            set
            {
                if (LoadAktie || (this.dividende.Waehrung != value))
                {
                    this.dividende.Waehrung = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public IEnumerable<Waehrungen> Waehrungen
        {
            get
            {
                return Enum.GetValues(typeof(Waehrungen)).Cast<Waehrungen>();
            }
        }

        #endregion
        public int WertpapierID
        {
            set { dividende.WertpapierID = value; }
        }
        public void Bearbeiten(int id)
        {
            LoadAktie = true;
            var _dividende = new DividendeAPI().LadeAnhandID(id);

            dividende = new Dividende
            {
                ID = _dividende.ID,
                RundungArt = _dividende.RundungArt
             };

            WertpapierID = _dividende.WertpapierID;
            Exdatum = _dividende.Exdatum;
            Zahldatum = _dividende.Zahldatum;
            Betrag = _dividende.Betrag;
            Waehrung = _dividende.Waehrung;
            BetragUmgerechnet = _dividende.BetragUmgerechnet;
            state = State.Bearbeiten;
            LoadAktie = false;
        }

        #region Validate
        private bool ValidateDatum(DateTime? datun)
        {
            var Validierung = new DividendeStammdatenValidierung();

            bool isValid = Validierung.ValidateDatum(datun, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Zahldatum", validationErrors);
            return isValid;
        }

        private bool ValidateBetrag(Double? betrag)
        {
            var Validierung = new DividendeStammdatenValidierung();

            bool isValid = Validierung.ValidateBetrag(betrag, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Betrag", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            dividende = new Dividende();
            Zahldatum = DateTime.Now;
            Exdatum = null;
            state = State.Neu;
            Betrag = null;
            Waehrung = Data.Types.WertpapierTypes.Waehrungen.Euro;
            this.RaisePropertyChanged();
        }

    }
}
