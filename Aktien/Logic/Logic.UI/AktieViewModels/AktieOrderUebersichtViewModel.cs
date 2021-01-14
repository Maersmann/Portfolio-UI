using Aktien.Data.Model.AktieModels;
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
        }

        #region Bindings

        public ICommand AktieGekauftCommand { get; set; }
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
            Messenger.Default.Send<OpenAktieGekauftViewMessage>(new OpenAktieGekauftViewMessage { AktieID = aktieID });
        }
        private void ExecuteEntfernenCommand()
        {
            new DepotAPI().EntferneGekaufteAktie(selectedOrderHistory.ID);
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
