using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
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

namespace Aktien.Logic.UI.ETFViewModels
{
    public class ETFStammdatenViewModel : ViewModelStammdaten<Wertpapier>, IViewModelStammdaten
    {

        public ETFStammdatenViewModel() : base(new EtfAPI())
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Cleanup();
        }

        protected override void ExecuteSaveCommand()
        {
            try
            {
                base.ExecuteSaveCommand();
            }
            catch (WertpapierSchonVorhandenException)
            {
                SendExceptionMessage("ETF ist schon vorhanden");
                return;
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.etf;




        public void ZeigeStammdatenAn(int id)
        {
            LoadAktie = true;
            var Loadaktie = new EtfAPI().Lade(id);
            data = new Wertpapier
            {
                ID = Loadaktie.ID,
                ETFInfo = new ETFInfo()
            };
            if (Loadaktie.ETFInfo != null)
            {
                ProfitTyp = Loadaktie.ETFInfo.ProfitTyp;
                ErmittentTyp = Loadaktie.ETFInfo.Emittent; 
                data.ETFInfo.ID = Loadaktie.ETFInfo.ID;
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
            get { return this.data.ISIN; }
            set
            {

                if (LoadAktie || !string.Equals(this.data.ISIN, value))
                {
                    ValidateISIN(value);
                    this.data.ISIN = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string Name
        {
            get { return this.data.Name; }
            set
            {
                if (LoadAktie || !string.Equals(this.data.Name, value))
                {
                    ValidateName(value);
                    this.data.Name = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string WKN
        {
            get { return this.data.WKN; }
            set
            {

                if (LoadAktie || !string.Equals(this.data.WKN, value))
                {
                    this.data.WKN = value;
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
            get { return data.ETFInfo.ProfitTyp; }
            set
            {
                if (LoadAktie || (this.data.ETFInfo.ProfitTyp != value))
                {
                    this.data.ETFInfo.ProfitTyp = value;
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
            get { return data.ETFInfo.Emittent; }
            set
            {
                if (LoadAktie || (this.data.ETFInfo.Emittent != value))
                {
                    this.data.ETFInfo.Emittent = value;
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
            data = new Wertpapier
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
