using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DepotLogic;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using Data.Model.DepotModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class EinnahmenAusgabenUebersichtViewModel : ViewModelLoadListData<EinnahmenAusgabenGesamtModel>
    {
        private EinnahmenAusgabenGesamtModel data;

        public EinnahmenAusgabenUebersichtViewModel()
        {
            data = new EinnahmenAusgabenGesamtModel();
            Title = "Einnahmen & Ausgaben Gesamtwerte";
            RegisterAktualisereViewMessage(StammdatenTypes.einnahmen.ToString());
            RegisterAktualisereViewMessage(StammdatenTypes.ausgaben.ToString());

        }

        public override async Task LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + "/api/depot/EinnahmenAusgaben");
                if (resp.IsSuccessStatusCode)
                {
                    var Response = await resp.Content.ReadAsAsync<Response<EinnahmenAusgabenGesamtModel>>();
                    data = Response.Data;
                }
                    
                RequestIsWorking = false;
            }
            OnPropertyChanged(nameof(EinnahmeEinzahlung));
            OnPropertyChanged(nameof(EinnahmeVerkauf));
            OnPropertyChanged(nameof(EinnahmeDividende));
            OnPropertyChanged(nameof(EinnahmeGesamt));
            OnPropertyChanged(nameof(AusgabeAuszahlung));
            OnPropertyChanged(nameof(AusgabeKauf));
            OnPropertyChanged(nameof(AusgabeGesamt));
            OnPropertyChanged(nameof(Differenz));
            OnPropertyChanged(nameof(EinnahmeZinsen));
        }

        #region Bindings
        public double EinnahmeEinzahlung => data.EinnahmeEinzahlung; 
        public double EinnahmeVerkauf => data.EinnahmeVerkauf;
        public double EinnahmeDividende => data.EinnahmeDividende;
        public double EinnahmeGesamt => data.EinnahmeGesamt;
        public double AusgabeAuszahlung => data.AusgabeAuszahlung; 
        public double AusgabeKauf =>  data.AusgabeKauf;
        public double AusgabeGesamt =>  data.AusgabeGesamt; 
        public double Differenz => data.Differenz;
        public double EinnahmeZinsen => data.EinnahmeZinsen;
        #endregion
    }
}
