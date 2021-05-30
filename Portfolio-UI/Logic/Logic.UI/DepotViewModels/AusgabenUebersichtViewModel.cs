using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.DepotModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class AusgabenUebersichtViewModel : ViewModelUebersicht<AusgabeModel>
    {
        public AusgabenUebersichtViewModel()
        {
            Title = "Übersicht aller Ausgaben";
            LoadData();
            RegisterAktualisereViewMessage(StammdatenTypes.ausgaben);
            RegisterAktualisereViewMessage(StammdatenTypes.buysell);
        }

        protected override int GetID() { return selectedItem.ID; }
        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.ausgaben; }

        public async override void LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+"/api/depot/Ausgaben");
                if (resp.IsSuccessStatusCode)
                    itemList = await resp.Content.ReadAsAsync<ObservableCollection<AusgabeModel>>();
            }
            this.RaisePropertyChanged("ItemList");
        }

    }
}
