using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.ETFModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.ETFViewModels
{
    public class ETFGesamtUebersichtViewModel : ViewModelUebersicht<ETFModel>
    {

        public ETFGesamtUebersichtViewModel()
        {
            Title = "Übersicht aller ETF's";
            LoadData();
            RegisterAktualisereViewMessage(StammdatenTypes.etf);
        }
        protected override int GetID() { return selectedItem.ID; }
        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.etf ; }


        public async override void LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+"/api/etf?aktiv=true");
                if (resp.IsSuccessStatusCode)
                    itemList = await resp.Content.ReadAsAsync<ObservableCollection<ETFModel>>();
            }
            this.RaisePropertyChanged("ItemList");
        }

        #region Binding
        public override ETFModel SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                base.SelectedItem = value;
                if (selectedItem != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedItem.ID, WertpapierTyp = selectedItem.WertpapierTyp }, messageToken);
                }
            }
        }
        #endregion

        #region Commands


        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL+ $"/api/Wertpapier/{selectedItem.ID}");
                if (resp.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    SendExceptionMessage("ETF ist im Depot vorhanden.");
                    return;
                }

            }
            Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = selectedItem.WertpapierTyp }, messageToken);
            itemList.Remove(selectedItem);
            this.RaisePropertyChanged("ItemList");
            SendInformationMessage("ETF gelöscht");
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewWertpapierUebersicht);
        }

        #endregion
    }
}
