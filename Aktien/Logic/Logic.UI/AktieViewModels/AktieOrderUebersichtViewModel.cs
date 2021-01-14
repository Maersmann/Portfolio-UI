using Aktien.Data.Model.AktieModels;
using Aktien.Data.Types;
using Aktien.Logic.Core.AktieLogic;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.DepotMessages;
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

namespace Aktien.Logic.UI.AktieViewModels
{
    public class AktieOrderUebersichtViewModel : ViewModelBasis
    {
        private ObservableCollection<OrderHistory> orderHistories;

        private OrderHistory selectedOrderHistory;

        private int aktieID;

        public AktieOrderUebersichtViewModel()
        {
            aktieID = 0;
            Messenger.Default.Register<LoadAktieOrderMessage>(this, m => ReceiveLoadAktieMessage(m));
            AktieGekauftCommand = new DelegateCommand(this.ExecuteAktieGekauftCommand, this.CanExecuteCommand);
            AktieVerkauftCommand = new DelegateCommand(this.ExecuteAktieVerkauftCommand, this.CanExecuteCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanSelectedItemExecuteCommand);
        }



        private void ReceiveLoadAktieMessage(LoadAktieOrderMessage m)
        {
            LoadData(m.AktieID);
        }

        public void LoadData(int inAktieID)
        {
            aktieID = inAktieID;
            orderHistories = new AktieAPI().LadeAlleOrdersDerAktie(aktieID);
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
            Messenger.Default.Send<OpenAktieGekauftViewMessage>(new OpenAktieGekauftViewMessage { AktieID = aktieID, BuySell = BuySell.Buy });
        }
        private void ExecuteAktieVerkauftCommand()
        {
            Messenger.Default.Send<OpenAktieGekauftViewMessage>(new OpenAktieGekauftViewMessage { AktieID = aktieID, BuySell = BuySell.Sell });
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
            return aktieID != 0;
        }
        private bool CanSelectedItemExecuteCommand()
        {
            return selectedOrderHistory != null;
        }
        #endregion
    }
}
