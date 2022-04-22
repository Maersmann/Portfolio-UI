using Aktien.Logic.Messages.AktieMessages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DepotViewModels.Page
{
    public class DepotUebersichtPageViewModel
    {
        public DepotUebersichtPageViewModel()
        {
            ShowDividendenViewCommand = new RelayCommand(ExecuteShowShowDividendenViewCommand);
            ShowOrderHistoryViewCommand = new RelayCommand(ExecuteShowOrderHistoryViewCommand);
        }

        private void ExecuteShowShowDividendenViewCommand()
        {
            Messenger.Default.Send(new OpenDetailViewMessage { ViewType = Data.Types.ViewType.viewDividendeUebersicht });
        }

        private void ExecuteShowOrderHistoryViewCommand()
        {
            Messenger.Default.Send(new OpenDetailViewMessage { ViewType = Data.Types.ViewType.viewOrderUebersicht });
        }

        public ICommand ShowDividendenViewCommand { get; set; }
        public ICommand ShowOrderHistoryViewCommand { get; set; }
    }
}
