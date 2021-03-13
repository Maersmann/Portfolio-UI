using Aktien.Data.Types;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.Core.DepotLogic.Models;
using Aktien.Logic.UI.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class EinnahmenAusgabenUebersichtViewModel : ViewModelLoadData
    {
        private EinnahmenAusgabenGesamtModel data;

        public EinnahmenAusgabenUebersichtViewModel()
        {
            Title = "Einnahmen & Ausgaben Gesamtwerte";
            LoadData();
            RegisterAktualisereViewMessage(StammdatenTypes.ausgaben);
            RegisterAktualisereViewMessage(StammdatenTypes.einnahmen);
            RegisterAktualisereViewMessage(StammdatenTypes.dividendeErhalten);
            RegisterAktualisereViewMessage(StammdatenTypes.buysell);
        }

        public override void LoadData()
        {
            data = new EinnahmeAusgabeAuswertungAPI().BerechneGesamtwerte();
            this.RaisePropertyChanged("EinnahmeEinzahlung");
            this.RaisePropertyChanged("EinnahmeVerkauf");
            this.RaisePropertyChanged("EinnahmeDividende");
            this.RaisePropertyChanged("EinnahmeGesamt");
            this.RaisePropertyChanged("AusgabeAuszahlung");
            this.RaisePropertyChanged("AusgabeKauf");
            this.RaisePropertyChanged("AusgabeGesamt");
            this.RaisePropertyChanged("Differenz");
        }

        #region Bindings
        public Double EinnahmeEinzahlung { get { return data.EinnahmeEinzahlung; } }
        public Double EinnahmeVerkauf{ get { return  data.EinnahmeVerkauf; } }
        public Double EinnahmeDividende { get { return data.EinnahmeDividende; } }
        public Double EinnahmeGesamt { get { return data.EinnahmeGesamt; } }
        public Double AusgabeAuszahlung { get { return data.AusgabeAuszahlung; } }
        public Double AusgabeKauf { get { return  data.AusgabeKauf; } }
        public Double AusgabeGesamt { get { return  data.AusgabeGesamt; } }
        public Double Differenz { get { return data.Differenz; } }
        #endregion
    }
}
