﻿using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Core.DepotLogic.Models;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class DepotUebersichtViewModel: ViewModelUebersicht<DepotGesamtUebersichtItem>
    {

        public DepotUebersichtViewModel()
        {
            Title = "Übersicht der Aktien im Depot";
            LoadData();
            OpenDividendeCommand = new DelegateCommand(this.ExecuteOpenDividendeCommandCommand, this.CanExecuteCommand);
            OpenReverseSplitCommand = new RelayCommand(() => ExecuteOpenReverseSplitCommand());
            RegisterAktualisereViewMessage(StammdatenTypes.buysell);
        }

        public override void LoadData()
        {
            var api = new DepotAPI();
            itemList = api.LadeFuerGesamtUebersicht();
            this.RaisePropertyChanged("ItemList");
            Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = WertpapierTypes.Aktie }, messageToken);
        }


        #region Bindings
        public override DepotGesamtUebersichtItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                ((DelegateCommand)OpenDividendeCommand).RaiseCanExecuteChanged();
                this.RaisePropertyChanged();
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
        protected  override bool CanExecuteCommand()
        {
            return base.CanExecuteCommand() && (selectedItem.WertpapierTyp.Equals(WertpapierTypes.Aktie));
        }

        private void ExecuteOpenDividendeCommandCommand()
        {
            Messenger.Default.Send<OpenDividendenUebersichtAuswahlMessage>(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = selectedItem.WertpapierID }, "DepotUebersicht");
        }

        private void ExecuteOpenReverseSplitCommand()
        {
            Messenger.Default.Send<OpenReverseSplitEintragenMessage>(new OpenReverseSplitEintragenMessage { DepotWertpapierID = selectedItem.WertpapierID}, "DepotUebersicht");
        }
        #endregion
    }
}
