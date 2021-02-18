using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeErhaltenViewModel : ViewModelStammdaten
    {
        private DividendeErhalten dividendeErhalten;
        private string dividendetext;

        public DividendeErhaltenViewModel()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            OpenAuswahlCommand = new RelayCommand(this.ExecuteOpenAuswahlCommand);
            Cleanup();
        }

        public void DividendeAusgewaehlt(int inID, double inBetrag, DateTime inDatum)
        {
            DateTimeFormatInfo fmt = (new CultureInfo("de-DE")).DateTimeFormat;
            dividendetext = inDatum.ToString("d", fmt) + " (" + inBetrag.ToString("N2") + ")";
            this.RaisePropertyChanged("DividendeText");
            DividendeID = inID;
        }

        public void Bearbeiten(int inID)
        {
            var dividendeLoad = new DividendeErhaltenAPI().LadeAnhandID(inID);

            dividendeErhalten = new DividendeErhalten
            {
                ID = dividendeLoad.ID,
                WertpapierID = dividendeLoad.WertpapierID,
                DividendeID = dividendeLoad.DividendeID,
                GesamtBrutto = dividendeLoad.GesamtBrutto,
                GesamtNetto = dividendeLoad.GesamtNetto, 
            };

            Bestand = dividendeLoad.Bestand;
            Datum = dividendeLoad.Datum;
            Quellensteuer = dividendeLoad.Quellensteuer;
            Wechselkurs = dividendeLoad.Umrechnungskurs;
            DividendeID = dividendeLoad.DividendeID;
            state = State.Bearbeiten;
        }

        private int DividendeID
        { 
            set 
            {
                dividendeErhalten.DividendeID = value;
                ValidateDividende(value);
                this.RaisePropertyChanged("DividendeText");
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            } 
        }

        private void ExecuteOpenAuswahlCommand()
        {
            Messenger.Default.Send<OpenDividendenAuswahlMessage>(new OpenDividendenAuswahlMessage { WertpapierID = dividendeErhalten.WertpapierID });
        }
        protected override void ExecuteSaveCommand()
        {
            var API = new DepotAPI();
            if (state == State.Neu)
            {
                API.NeueDividendeErhalten(dividendeErhalten);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Erhaltene Dividende gespeichert." }, "ErhalteneDividendeStammdaten");

            }
            else
            {
                API.AktualisiereDividendeErhalten(dividendeErhalten);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Erhaltene Dividende aktualisiert." }, "ErhalteneDividendeStammdaten");
            }
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewEinnahmenUebersicht);
        }

        public void WertpapierID(int inWertpapierID)
        {
            dividendeErhalten.WertpapierID = inWertpapierID;
        }

        #region Bindings
        public ICommand OpenAuswahlCommand { get; set; }
        public Double? Bestand
        {
            get 
            {
                if (dividendeErhalten.Bestand == -1)
                    return null;
                else
                    return dividendeErhalten.Bestand; 
            }
            set 
            {
                if (this.dividendeErhalten.Bestand != value)
                {
                    ValidateBestand(value);
                    this.dividendeErhalten.Bestand = value.GetValueOrDefault(-1);
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public DateTime? Datum
        {
            get
            {
                if (dividendeErhalten.Datum == DateTime.MinValue)
                    return null;
                else
                    return dividendeErhalten.Datum;
            }
            set
            {
                if (this.dividendeErhalten.Datum != value)
                {
                    ValidateDatum(value);
                    this.dividendeErhalten.Datum = value.GetValueOrDefault(DateTime.MinValue);
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string DividendeText
        {
            get { return dividendetext; }
        }
        public Double? Quellensteuer
        {
            get { return dividendeErhalten.Quellensteuer; }
            set
            {
                if (this.dividendeErhalten.Quellensteuer != value)
                {
                    this.dividendeErhalten.Quellensteuer = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public Double? Wechselkurs
        {
            get { return dividendeErhalten.Umrechnungskurs; }
            set
            {
                if (this.dividendeErhalten.Umrechnungskurs != value)
                {
                    this.dividendeErhalten.Umrechnungskurs = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged("WechsellkursHasValue");
                }
            }
        }

        public IEnumerable<DividendenRundungTypes> RundungTypes
        {
            get
            {
                return Enum.GetValues(typeof(DividendenRundungTypes)).Cast<DividendenRundungTypes>();
            }
        }

        public DividendenRundungTypes RundungTyp
        {
            get { return dividendeErhalten.RundundArt; }
            set
            {
                if (LoadAktie || (this.dividendeErhalten.RundundArt != value))
                {
                    this.dividendeErhalten.RundundArt = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public Double? GesamtNettoUmgerechnet
        {
            get { return dividendeErhalten.GesamtNettoUmgerechnet; }
        }

        public Double? GesamtNettoUmgerechnetUngerundet
        {
            get { return dividendeErhalten.GesamtNettoUmgerechnetUngerundet; }
        }

        public bool WechsellkursHasValue { get { return this.dividendeErhalten.Umrechnungskurs.GetValueOrDefault(0)>0; } }
        #endregion

        #region Validate
        private bool ValidateBestand(Double? inBestand)
        {
            var Validierung = new DividendeErhaltenValidierung();

            bool isValid = Validierung.ValidateBestand(inBestand, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Bestand", validationErrors);
            return isValid;
        }

        private bool ValidateDatum(DateTime? inDatum)
        {
            var Validierung = new DividendeErhaltenValidierung();

            bool isValid = Validierung.ValidateDatum(inDatum, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Datum", validationErrors);
            return isValid;
        }

        private bool ValidateDividende(int inID)
        {
            var Validierung = new DividendeErhaltenValidierung();

            bool isValid = Validierung.ValidateDividende(inID, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "DividendeText", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            dividendeErhalten = new DividendeErhalten();
            dividendetext = "";
            DividendeID = -1;
            Bestand = -1;
            Datum = DateTime.Now;
            state = State.Neu;
            Quellensteuer = null;
            Wechselkurs = null;
            RundungTyp = DividendenRundungTypes.Normal;
            this.RaisePropertyChanged();
        }
    }
}

