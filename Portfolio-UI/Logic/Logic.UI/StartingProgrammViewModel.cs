using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Aktien.Logic.UI
{
    public class StartingProgrammViewModel : ViewModelBasis
    {
        public StartingProgrammViewModel()
        {
            Title = "Loading...";
            CheckServerIsOnlineCommand = new RelayCommand(() => ExecuteCheckServerIsOnlineCommand());
        }

        private void ExecuteCheckServerIsOnlineCommand()
        {
            new BackendHelper().CheckServerIsOnline();

            if (GlobalVariables.ServerIsOnline)
            {
                ViewModelLocator locator = new ViewModelLocator();
                locator.Main.RaisePropertyChanged("MenuIsEnabled");
                Messenger.Default.Send<OpenViewMessage>(new OpenViewMessage { ViewType = ViewType.viewWertpapierUebersicht });
            }
                
            Messenger.Default.Send(new CloseViewMessage(), "StartingProgramm");
        }

        public ICommand CheckServerIsOnlineCommand { get; private set; }

        
    }
}
