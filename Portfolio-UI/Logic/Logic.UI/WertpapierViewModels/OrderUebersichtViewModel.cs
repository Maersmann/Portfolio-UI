using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DepotMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using Data.Model.WertpapierModels;
using GalaSoft.MvvmLight.Command;
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
    public class OrderUebersichtViewModel : ViewModelUebersicht<OrderUebersichtModel, StammdatenTypes>
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
            AktieGekauftCommand = new DelegateCommand(ExecuteAktieGekauftCommand, CanExecuteCommand);
            AktieVerkauftCommand = new DelegateCommand(ExecuteAktieVerkauftCommand, CanExecuteAktieVerkaufCommand);
            EntfernenCommand = new DelegateCommand(ExecuteEntfernenCommand, CanSelectedItemExecuteCommand);
            RegisterAktualisereViewMessage(StammdatenTypes.buysell.ToString());
            CheckCanExecuteAktieVerkaufCommand();
        }

        protected override string GetREST_API() { return $"/api/Wertpapier/{wertpapierID}/Orders/"; }
        protected override bool WithPagination() { return true; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.buysell; }
        protected override bool LoadingOnCreate() => false;

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

        public override Task LoadData()
        {
            LoadData(wertpapierID);
            return Task.CompletedTask;
        }

        public override async void LoadData(int id)
        {
            wertpapierID = id;

            await base.LoadData();

            ((DelegateCommand)AktieGekauftCommand).RaiseCanExecuteChanged();
            CheckCanExecuteAktieVerkaufCommand();
        }


        #region Bindings

        public ICommand AktieGekauftCommand { get; set; }
        public ICommand AktieVerkauftCommand { get; set; }

        #endregion

        #region Commands
        public async void CheckCanExecuteAktieVerkaufCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/Depot/Wertpapier/{wertpapierID}/Exist");
            if (resp.IsSuccessStatusCode)
            {
                Response<bool> ExistResponse = await resp.Content.ReadAsAsync<Response<bool>>();
                canExecuteAktieVerkaufCommand = ExistResponse.Data;
            }
            else
            {
                canExecuteAktieVerkaufCommand = false;
            }

            RequestIsWorking = false;
            ((DelegateCommand)AktieVerkauftCommand).RaiseCanExecuteChanged();
        }
        private void ExecuteAktieGekauftCommand()
        {
            Messenger.Default.Send(new OpenWertpapierGekauftViewMessage { WertpapierID = wertpapierID, BuySell = BuySell.Buy, WertpapierTypes = wertpapierTypes }, messagtoken);
        }
        private void ExecuteAktieVerkauftCommand()
        {
            Messenger.Default.Send(new OpenWertpapierGekauftViewMessage { WertpapierID = wertpapierID, BuySell = BuySell.Sell, WertpapierTypes = wertpapierTypes }, messagtoken);
        }
        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/Depot/Order/{SelectedItem.ID}/Delete?buysell={SelectedItem.BuySell}");
                RequestIsWorking = false;
                if (resp.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    return;
                }

            }
            base.ExecuteEntfernenCommand();
        }

        protected override bool CanExecuteCommand()
        {
            return wertpapierID != 0;
        }
        private bool CanSelectedItemExecuteCommand()
        {
            return (SelectedItem != null) && ( ItemList.IndexOf(SelectedItem) == 0 );
        }

        private bool CanExecuteAktieVerkaufCommand()
        {
            return canExecuteAktieVerkaufCommand;
        }

        #endregion

    }
}
