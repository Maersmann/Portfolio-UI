﻿using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types;
using Aktien.Data.Types.DepotTypes;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Core.Validierung.Base;
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

namespace Aktien.Logic.UI.DepotViewModels
{
    public class EinnahmeStammdatenViewModel : ViewModelStammdaten<Einnahme>, IViewModelStammdaten
    {
        public EinnahmeStammdatenViewModel()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Cleanup();
        }

        public int DepotID{ set { data.DepotID = value; } }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.einnahmen;

        #region Bindings
        public IEnumerable<EinnahmeArtTypes> EinnahmeTypes
        {
            get
            {
                return Enum.GetValues(typeof(EinnahmeArtTypes)).Cast<EinnahmeArtTypes>();
            }
        }
     
        public EinnahmeArtTypes EinnahmeTyp
        {
            get
            {
                return data.Art;
            }
            set 
            {
                if ((this.data.Art != value))
                {
                    data.Art = value;
                    this.RaisePropertyChanged();
                }
            }

        }

        public DateTime? Datum
        {
            get
            {
                return data.Datum;
            }
            set
            {
                if (!DateTime.Equals(this.data.Datum, value))
                {
                    ValidateDatum(value);
                    this.data.Datum = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public double? Betrag
        {
            get
            {
                return data.Betrag;
            }
            set
            {
                if (LoadAktie || (this.data.Betrag != value))
                {
                    ValidateBetrag(value);
                    this.data.Betrag = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public String Beschreibung
        {
            get
            {
                return data.Beschreibung;
            }
            set
            {
                if (this.data.Beschreibung != value)
                {
                    this.data.Beschreibung = value;
                    this.RaisePropertyChanged();
                }
            }
        }


        #endregion

        #region Commands
        protected override void ExecuteSaveCommand()
        {
            var Depot = new DepotAPI();
            Depot.NeueEinnahme(data.Betrag, data.Datum, data.Art, data.DepotID, null, data.Beschreibung);

            Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Einnahme gespeichert." }, GetStammdatenTyp());
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.einnahmen);
        }

        #endregion

        #region Validate
        private bool ValidateBetrag(Double? betrag )
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateBetrag(betrag, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Betrag", validationErrors);
            return isValid;
        }

        private bool ValidateDatum(DateTime? datum)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateDatum(datum, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Datum", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            state = State.Neu;
            data = new Einnahme();
            Betrag = null;
            Datum = DateTime.Now;
            DepotID = 1;
            Beschreibung = "";
        }

        public void ZeigeStammdatenAn(int id)
        {
            throw new NotImplementedException();
        }
    }
}
