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
using Base.Logic.Wrapper;
using Data.Model.WertpapierModels;

using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.UtilMessages;
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
using Logic.Messages.SparplanMessages;

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
                WeakReferenceMessenger.Default.Register<LoadWertpapierOrderMessage, string>(this, value, (r, m) => ReceiveLoadAktieMessage(m));
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
             WeakReferenceMessenger.Default.Send(new OpenWertpapierGekauftViewMessage { WertpapierID = wertpapierID, BuySell = BuySell.Buy, WertpapierTypes = wertpapierTypes }, messagtoken);
        }
        private void ExecuteAktieVerkauftCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenWertpapierGekauftViewMessage { WertpapierID = wertpapierID, BuySell = BuySell.Sell, WertpapierTypes = wertpapierTypes }, messagtoken);
        }
        protected override void ExecuteEntfernenCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenBestaetigungViewMessage
            {
                Beschreibung = "Soll der Eintrag gelöscht werden?",
                Command = async () =>
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
                        if (resp.IsSuccessStatusCode)
                        {
                            base.ExecuteEntfernenCommand();
                        }

                    }
                }
            }, "OrderUebersicht");
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

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            WeakReferenceMessenger.Default.Unregister<LoadWertpapierOrderMessage, string>(this,messageToken);

        }

    }
}
