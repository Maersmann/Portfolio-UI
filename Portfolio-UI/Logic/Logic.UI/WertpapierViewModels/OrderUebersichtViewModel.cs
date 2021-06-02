using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DepotMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.WertpapierModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.WertpapierViewModels
{
    public class OrderUebersichtViewModel : ViewModelUebersicht<OrderUebersichtModel>
    {

        private int wertpapierID;
        private WertpapierTypes wertpapierTypes;
        private string messagtoken;
        private bool canExecuteAktieVerkaufCommand;

        public OrderUebersichtViewModel()
        {
            canExecuteAktieVerkaufCommand = false;          
            Title = "Übersicht der Order";
            wertpapierID = 0;
            wertpapierTypes = WertpapierTypes.Aktie;
            AktieGekauftCommand = new DelegateCommand(this.ExecuteAktieGekauftCommand, this.CanExecuteCommand);
            AktieVerkauftCommand = new DelegateCommand(this.ExecuteAktieVerkauftCommand, this.CanExecuteAktieVerkaufCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanSelectedItemExecuteCommand);
            RegisterAktualisereViewMessage(StammdatenTypes.buysell);
            CheckCanExecuteAktieVerkaufCommand();
        }

        public override string MessageToken
        {
            set
            {
                Messenger.Default.Register<LoadWertpapierOrderMessage>(this, value, m => ReceiveLoadAktieMessage(m));
                messagtoken = value;
            }
        }
        private void ReceiveLoadAktieMessage(LoadWertpapierOrderMessage m)
        {
            wertpapierTypes = m.WertpapierTyp;
            LoadData(m.WertpapierID);
        }

        public override void LoadData()
        {
            LoadData(wertpapierID);
        }
        public override async void LoadData(int id)
        {
            wertpapierID = id;

            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/Wertpapier/{wertpapierID}/Orders/");
            if (resp.IsSuccessStatusCode)
            {
                itemList = await resp.Content.ReadAsAsync<ObservableCollection<OrderUebersichtModel>>();
            }
            this.RaisePropertyChanged("ItemList");

            ((DelegateCommand)AktieGekauftCommand).RaiseCanExecuteChanged();
            CheckCanExecuteAktieVerkaufCommand();

        }

        public async void CheckCanExecuteAktieVerkaufCommand()
        {
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/Depot/Wertpapier/{wertpapierID}/Exist");
            if (resp.IsSuccessStatusCode)
            {
                canExecuteAktieVerkaufCommand = await resp.Content.ReadAsAsync<bool>();
            }
            else
                canExecuteAktieVerkaufCommand = false;
            ((DelegateCommand)AktieVerkauftCommand).RaiseCanExecuteChanged();
        }

        #region Bindings

        public ICommand AktieGekauftCommand { get; set; }
        public ICommand AktieVerkauftCommand { get; set; }

        #endregion

        #region Commands
        private void ExecuteAktieGekauftCommand()
        {
            Messenger.Default.Send<OpenWertpapierGekauftViewMessage>(new OpenWertpapierGekauftViewMessage { WertpapierID = wertpapierID, BuySell = BuySell.Buy, WertpapierTypes = wertpapierTypes }, messagtoken);
        }
        private void ExecuteAktieVerkauftCommand()
        {
            Messenger.Default.Send<OpenWertpapierGekauftViewMessage>(new OpenWertpapierGekauftViewMessage { WertpapierID = wertpapierID, BuySell = BuySell.Sell, WertpapierTypes = wertpapierTypes }, messagtoken);
        }
        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/Depot/Order/{selectedItem.ID}/Delete?buysell={selectedItem.BuySell}");
                if (resp.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {            
                    return;
                }

            }
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.buysell);
            base.ExecuteEntfernenCommand();
        }

        protected override bool CanExecuteCommand()
        {
            return wertpapierID != 0;
        }
        private bool CanSelectedItemExecuteCommand()
        {
            return (selectedItem != null) && ( itemList.IndexOf(SelectedItem) == 0 );
        }

        private bool CanExecuteAktieVerkaufCommand()
        {
            return canExecuteAktieVerkaufCommand;
        }

        #endregion

    }
}
