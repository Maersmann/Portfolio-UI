using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.DepotModels;
using GalaSoft.MvvmLight.CommandWpf;
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
    public class EinnahmenUebersichtViewModel : ViewModelUebersicht<EinnahmeModel>
    {
        public EinnahmenUebersichtViewModel()
        {
            Title = "Übersicht aller Einnahmen";
            LoadData();
            RegisterAktualisereViewMessage(StammdatenTypes.einnahmen);
            RegisterAktualisereViewMessage(StammdatenTypes.dividendeErhalten);
            RegisterAktualisereViewMessage(StammdatenTypes.steuer);
            RegisterAktualisereViewMessage(StammdatenTypes.buysell);
        }

        protected override int GetID() { return selectedItem.ID; }
        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.einnahmen; }

        public async override void LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync("https://localhost:5001/api/depot/Einnahmen");
                if (resp.IsSuccessStatusCode)
                    itemList = await resp.Content.ReadAsAsync<ObservableCollection<EinnahmeModel>>();
            }
            this.RaisePropertyChanged("ItemList");
        }



    }
}
