using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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

        private static void ExecuteCheckServerIsOnlineCommand()
        {
            BackendHelper.CheckServerIsOnline();
            WeakReferenceMessenger.Default.Send(new CloseViewMessage(), "StartingProgramm");
            if (GlobalVariables.ServerIsOnline)
            {
                 WeakReferenceMessenger.Default.Send(new OpenLoginViewMessage { });
            }
            else
            {
                 WeakReferenceMessenger.Default.Send(new CloseApplicationMessage { });
            }
                
            
        }

        public ICommand CheckServerIsOnlineCommand { get; private set; }

        
    }
}
