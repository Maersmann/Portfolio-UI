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
    public class DerivateStammdatenViewModel : ViewModelStammdaten<Wertpapier>,IViewModelStammdaten
    {

        public DerivateStammdatenViewModel() : base(new DerivateAPI())
        {
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
                SendExceptionMessage("Derivate ist schon vorhanden");
                return;
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.derivate;

        public void ZeigeStammdatenAn(int id)
        {
            LoadAktie = true;
            var Loadaktie = new DerivateAPI().Lade(id);
            data = new Wertpapier
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
            data = new Wertpapier();
            ISIN = "";
            Name = "";
            WKN = "";
        }
    }
}
