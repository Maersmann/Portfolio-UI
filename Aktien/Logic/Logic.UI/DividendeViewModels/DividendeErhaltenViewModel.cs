using Aktien.Data.Model.AktienModels;
using Aktien.Data.Types;
using Aktien.Logic.Core.Depot;
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
            dividendeErhalten = new DividendeErhalten();
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            OpenAuswahlCommand = new RelayCommand(this.ExecuteOpenAuswahlCommand);
            Bestand = -1;
            Datum = DateTime.Now;
            Quellensteuer = null;
            Wechselkurs = null;
            dividendetext = "";
            DividendeID = -1;
        }

        public void DividendeAusgewaehlt(int inID, double inBetrag, DateTime inDatum)
        {
            DateTimeFormatInfo fmt = (new CultureInfo("de-DE")).DateTimeFormat;
            dividendetext = inDatum.ToString("d", fmt) + " (" + inBetrag.ToString("N2") + ")";
            this.RaisePropertyChanged("DividendeText");
            DividendeID = inID;
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
            Messenger.Default.Send<OpenDividendenAuswahlMessage>(new OpenDividendenAuswahlMessage { AktieID = dividendeErhalten.AktieID });
        }
        protected override void ExecuteSaveCommand()
        {
            var API = new DepotAPI();
            if (state == State.Neu)
            {
                API.NeueDividendenErhalten(dividendeErhalten);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Erhaltene Dividende gespeichert." }, "ErhalteneDividendeStammdaten");

            }
            else
            {
                //API.Aktualisiere(dividende.Betrag, dividende.Datum, dividende.ID);
                // Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende aktualisiert." });

            }
        }

        public void AktieID(int aktieID)
        {
            dividendeErhalten.AktieID = aktieID;
        }

        #region Bindings
        public ICommand OpenAuswahlCommand { get; set; }
        public int? Bestand
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
                }
            }
        }
        #endregion

        #region Validate
        private bool ValidateBestand(int? inBestand)
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
            state = State.Neu;
            this.RaisePropertyChanged();
        }
    }
}

