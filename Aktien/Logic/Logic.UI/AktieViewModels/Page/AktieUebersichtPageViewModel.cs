using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.AktieViewModels.Page
{
    public class AktieUebersichtPageViewModel : ViewModelBasis
    {
        public AktieUebersichtPageViewModel()
        {
            ShowDividendenViewCommand = new RelayCommand(this.ExecuteShowShowDividendenViewCommand);
            ShowOrderHistoryViewCommand = new RelayCommand(this.ExecuteShowOrderHistoryViewCommand);
        }

        private void ExecuteShowShowDividendenViewCommand()
        {
            Messenger.Default.Send<OpenDetailViewMessage>(new OpenDetailViewMessage { ViewType = Data.Types.ViewType.viewDividendeUebersicht });
        }

        private void ExecuteShowOrderHistoryViewCommand()
        {
            Messenger.Default.Send<OpenDetailViewMessage>(new OpenDetailViewMessage { ViewType = Data.Types.ViewType.viewOrderUebersicht });
        }

        public ICommand ShowDividendenViewCommand { get; set; }
        public ICommand ShowOrderHistoryViewCommand { get; set; }
    }


}
