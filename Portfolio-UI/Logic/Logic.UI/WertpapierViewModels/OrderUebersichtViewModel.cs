﻿using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DepotMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
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

            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/Wertpapier/{wertpapierID}/Orders/");
            if (resp.IsSuccessStatusCode)
            {
                itemList = await resp.Content.ReadAsAsync<ObservableCollection<OrderUebersichtModel>>();
            }
            RequestIsWorking = false;
            RaisePropertyChanged("ItemList");

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
                canExecuteAktieVerkaufCommand = await resp.Content.ReadAsAsync<bool>();
            }
            else
                canExecuteAktieVerkaufCommand = false;
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
                HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/Depot/Order/{selectedItem.ID}/Delete?buysell={selectedItem.BuySell}");
                RequestIsWorking = false;
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
