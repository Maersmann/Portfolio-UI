﻿using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Base.Logic.ViewModels;
using Data.Model.DepotModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
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
    public class DepotUebersichtViewModel: ViewModelUebersicht<DepotGesamtUebersichtModel, StammdatenTypes>
    {

        public DepotUebersichtViewModel()
        {
            Title = "Übersicht der Aktien im Depot";
            LoadData();
            OpenDividendeCommand = new DelegateCommand(ExecuteOpenDividendeCommandCommand, this.CanExecuteCommand);
            OpenReverseSplitCommand = new RelayCommand(() => ExecuteOpenReverseSplitCommand());
            RegisterAktualisereViewMessage(StammdatenTypes.buysell.ToString());
        }

        protected override string GetREST_API() { return $"/api/Depot"; }

        /*TODO: LoadWertpapier???
        public async override void LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + "");
                if (resp.IsSuccessStatusCode)
                {
                    itemList = await resp.Content.ReadAsAsync<ObservableCollection<DepotGesamtUebersichtModel>>();
                }
            }
            base.LoadData();
            Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = WertpapierTypes.Aktie }, messageToken);
        }
        */

        protected override bool OnFilterTriggered(object item)
        {
            if (item is DepotGesamtUebersichtModel depot)
            {
                return depot.Bezeichnung.ToLower().Contains(filtertext.ToLower());
            }
            return true;
        }

        #region Bindings
        public override DepotGesamtUebersichtModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                ((DelegateCommand)OpenDividendeCommand).RaiseCanExecuteChanged();
                RaisePropertyChanged();
                if (selectedItem != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedItem.WertpapierID, WertpapierTyp = selectedItem.WertpapierTyp }, messageToken);
                }
            }
        }

        public ICommand OpenDividendeCommand { get; set; }
        public ICommand OpenReverseSplitCommand { get; set; }
        #endregion

        #region Commands
        protected override bool CanExecuteCommand()
        {
            return base.CanExecuteCommand() && (selectedItem.WertpapierTyp.Equals(WertpapierTypes.Aktie));
        }

        private void ExecuteOpenDividendeCommandCommand()
        {
            Messenger.Default.Send(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = selectedItem.WertpapierID }, "DepotUebersicht");
        }

        private void ExecuteOpenReverseSplitCommand()
        {
            Messenger.Default.Send(new OpenReverseSplitEintragenMessage { DepotWertpapierID = selectedItem.WertpapierID}, "DepotUebersicht");
        }
        #endregion
    }
}
