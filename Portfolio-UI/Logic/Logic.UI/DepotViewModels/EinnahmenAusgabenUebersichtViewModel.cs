using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.DepotModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class EinnahmenAusgabenUebersichtViewModel : ViewModelLoadData
    {
        private EinnahmenAusgabenGesamtModel data;

        public EinnahmenAusgabenUebersichtViewModel()
        {
            data = new EinnahmenAusgabenGesamtModel();
            Title = "Einnahmen & Ausgaben Gesamtwerte";
            LoadData();
            RegisterAktualisereViewMessage(StammdatenTypes.ausgaben);
            RegisterAktualisereViewMessage(StammdatenTypes.einnahmen);
            RegisterAktualisereViewMessage(StammdatenTypes.dividendeErhalten);
            RegisterAktualisereViewMessage(StammdatenTypes.buysell);
        }

        public async override void LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync("https://localhost:5001/api/depot/EinnahmenAusgaben");
                if (resp.IsSuccessStatusCode)
                    data = await resp.Content.ReadAsAsync<EinnahmenAusgabenGesamtModel>();
            }
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
