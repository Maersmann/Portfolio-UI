using Aktien.Data.Model.WertpapierModels;
using Aktien.Data.Types;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.DepotMessages;
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

namespace Aktien.Logic.UI.WertpapierViewModels
{
    public class OrderUebersichtViewModel : ViewModelBasis
    {
        private ObservableCollection<OrderHistory> orderHistories;

        private OrderHistory selectedOrderHistory;

        private int wertpapierID;

        public OrderUebersichtViewModel()
        {
            wertpapierID = 0;
            Messenger.Default.Register<LoadWertpapierOrderMessage>(this, m => ReceiveLoadAktieMessage(m));
            AktieGekauftCommand = new DelegateCommand(this.ExecuteAktieGekauftCommand, this.CanExecuteCommand);
            AktieVerkauftCommand = new DelegateCommand(this.ExecuteAktieVerkauftCommand, this.CanExecuteCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanSelectedItemExecuteCommand);
        }



        private void ReceiveLoadAktieMessage(LoadWertpapierOrderMessage m)
        {
            LoadData(m.WertpapierID);
        }

        public void LoadData(int inWertpapierID)
        {
            wertpapierID = inWertpapierID;
            orderHistories = new AktieAPI().LadeAlleOrdersDerAktie(wertpapierID);
            this.RaisePropertyChanged("OrderHistories");
            ((DelegateCommand)AktieGekauftCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)AktieVerkauftCommand).RaiseCanExecuteChanged();
        }

        #region Bindings

        public ICommand AktieGekauftCommand { get; set; }
        public ICommand AktieVerkauftCommand { get; set; }
        public ICommand BearbeitenCommand { get; set; }
        public ICommand EntfernenCommand{get;set;}

        public OrderHistory SelectedOrderHistory
        {
            get
            {
                return selectedOrderHistory;
            }
            set
            {
                selectedOrderHistory = value;
                ((DelegateCommand)EntfernenCommand).RaiseCanExecuteChanged();
                this.RaisePropertyChanged();
            }
        }
        public IEnumerable<OrderHistory> OrderHistories
        {
            get
            {
                return orderHistories;
            }
        }
        #endregion

        #region Commands
        private void ExecuteAktieGekauftCommand()
        {
            Messenger.Default.Send<OpenAktieGekauftViewMessage>(new OpenAktieGekauftViewMessage { WertpapierID = wertpapierID, BuySell = BuySell.Buy });
        }
        private void ExecuteAktieVerkauftCommand()
        {
            Messenger.Default.Send<OpenAktieGekauftViewMessage>(new OpenAktieGekauftViewMessage { WertpapierID = wertpapierID, BuySell = BuySell.Sell });
        }
        private void ExecuteEntfernenCommand()
        {
            if (selectedOrderHistory.BuySell == BuySell.Buy)
            {
                new DepotAPI().EntferneGekaufteAktie(selectedOrderHistory.ID);
            }
            else
            {
                new DepotAPI().EntferneVerkaufteAktie(selectedOrderHistory.ID);
            }
            
            orderHistories.Remove(selectedOrderHistory);
            this.RaisePropertyChanged("SelectedOrderHistory");
        }

        private bool CanExecuteCommand()
        {
            return wertpapierID != 0;
        }
        private bool CanSelectedItemExecuteCommand()
        {
            return selectedOrderHistory != null;
        }
        #endregion
    }
}
