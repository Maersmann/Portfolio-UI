using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;
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
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Cleanup();
        }


        protected override void ExecuteSaveCommand()
        {
            var api = new EtfAPI();
            if (state.Equals(State.Neu))
            {
                try
                {
                    api.Speichern(new Wertpapier() { ISIN = etf.ISIN, Name = etf.Name, WKN = etf.WKN, ETFInfo = new ETFInfo { Emittent = etf.ETFInfo.Emittent, ProfitTyp = etf.ETFInfo.ProfitTyp } });
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "ETF gespeichert." }, "ETFStammdaten");
                }
                catch (WertpapierSchonVorhandenException)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = false, Message = "ETF ist schon vorhanden." }, "ETFStammdaten");
                    return;
                }
            }
            else
            {
                api.Aktualisieren(etf);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "ETF aktualisiert." }, "ETFStammdaten");
            }
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewWertpapierUebersicht);
        }

        public void Bearbeiten(int id)
        {
            LoadAktie = true;
            var Loadaktie = new EtfAPI().LadeAnhandID(id);
            etf = new Wertpapier
            {
                ID = Loadaktie.ID,
                ETFInfo = new ETFInfo()
            };
            if (Loadaktie.ETFInfo != null)
            {
                ProfitTyp = Loadaktie.ETFInfo.ProfitTyp;
                ErmittentTyp = Loadaktie.ETFInfo.Emittent; 
                etf.ETFInfo.ID = Loadaktie.ETFInfo.ID;
            }
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

        public IEnumerable<ProfitTypes> ProfitTypes
        {
            get
            {
                return Enum.GetValues(typeof(ProfitTypes)).Cast<ProfitTypes>();
            }
        }
        public ProfitTypes ProfitTyp
        {
            get { return etf.ETFInfo.ProfitTyp; }
            set
            {
                if (LoadAktie || (this.etf.ETFInfo.ProfitTyp != value))
                {
                    this.etf.ETFInfo.ProfitTyp = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<ErmittentTypes> ErmittentTypes
        {
            get
            {
                return Enum.GetValues(typeof(ErmittentTypes)).Cast<ErmittentTypes>();
            }
        }
        public ErmittentTypes ErmittentTyp
        {
            get { return etf.ETFInfo.Emittent; }
            set
            {
                if (LoadAktie || (this.etf.ETFInfo.Emittent != value))
                {
                    this.etf.ETFInfo.Emittent = value;
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
            etf = new Wertpapier
            {
                ETFInfo = new ETFInfo()
            };
            ISIN = "";
            Name = "";
            WKN = "";
            ProfitTyp = Data.Types.WertpapierTypes.ProfitTypes.Thesaurierend;
            ErmittentTyp = Data.Types.WertpapierTypes.ErmittentTypes.iShares;
        }
    }
}
