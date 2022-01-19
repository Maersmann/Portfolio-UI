using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Base;
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
            Messenger.Default.Send(new CloseViewMessage(), "StartingProgramm");
            if (GlobalVariables.ServerIsOnline)
            {
                Messenger.Default.Send(new OpenLoginViewMessage { });
            }
            else
            {
                Messenger.Default.Send(new CloseApplicationMessage { });
            }
                
            
        }

        public ICommand CheckServerIsOnlineCommand { get; private set; }

        
    }
}
