using Aktien.Logic.Core;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.AuswertungModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace Logic.UI.AuswertungViewModels
{
    public class DividendeWertpapierAuswertungViewModel : ViewModelUebersicht<DividendeWertpapierAuswertungModel>
    {

        public DividendeWertpapierAuswertungViewModel()
        {
            Title = "Auswertung Dividende je Wertpapier";
            LoadData();
        }


        public async override void LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + "/api/auswertung/dividenden/Wertpapiere");
                if (resp.IsSuccessStatusCode)
                    itemList = await resp.Content.ReadAsAsync<ObservableCollection<DividendeWertpapierAuswertungModel>>();
            }
            this.RaisePropertyChanged("ItemList");
        }

    }
}
