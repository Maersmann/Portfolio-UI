﻿using Aktien.Logic.Messages.AktieMessages;
using Base.Logic.ViewModels;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.ETFViewModels.Page
{
    public class ETFUebersichtPageViewModel : ViewModelBasis
    {
        public ETFUebersichtPageViewModel()
        {
            ShowDividendenViewCommand = new RelayCommand(ExecuteShowShowDividendenViewCommand);
            ShowOrderHistoryViewCommand = new RelayCommand(ExecuteShowOrderHistoryViewCommand);
        }

        private void ExecuteShowShowDividendenViewCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenDetailViewMessage { ViewType = Data.Types.ViewType.viewDividendeUebersicht });
        }

        private void ExecuteShowOrderHistoryViewCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenDetailViewMessage { ViewType = Data.Types.ViewType.viewOrderUebersicht });
        }

        public ICommand ShowDividendenViewCommand { get; set; }
        public ICommand ShowOrderHistoryViewCommand { get; set; }
    }


}
