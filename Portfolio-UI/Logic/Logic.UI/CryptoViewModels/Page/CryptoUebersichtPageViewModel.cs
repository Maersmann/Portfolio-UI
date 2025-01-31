using Aktien.Data.Types;
using Aktien.Logic.Messages.AktieMessages;
using Base.Logic.ViewModels;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Logic.UI.CryptoViewModels.Page
{
    public class CryptoUebersichtPageViewModel : ViewModelBasis
    {
        public CryptoUebersichtPageViewModel()
        {
            ShowOrderHistoryViewCommand = new RelayCommand(this.ExecuteShowOrderHistoryViewCommand);
        }

        private void ExecuteShowOrderHistoryViewCommand()
        {
            WeakReferenceMessenger.Default.Send(new OpenDetailViewMessage { ViewType = ViewType.viewOrderUebersicht });
        }

        public ICommand ShowOrderHistoryViewCommand { get; set; }
    }
}
