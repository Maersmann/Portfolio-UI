using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
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
    public class StartingProgrammViewModel:ViewModelBasis
    {
        public StartingProgrammViewModel()
        {
            Title = "Loading...";
            CheckServerIsOnlineCommand = new RelayCommand(() => ExecuteCheckServerIsOnlineCommand());
        }

        private void ExecuteCheckServerIsOnlineCommand()
        {
            CheckServerIsOnline();
            if (GlobalVariables.ServerIsOnline)
            {
                ViewModelLocator locator = new ViewModelLocator();
                locator.Main.RaisePropertyChanged("MenuIsEnabled");
                locator.Main.RaisePropertyChanged("CanLoadData");
                Messenger.Default.Send<OpenViewMessage>(new OpenViewMessage { ViewType = ViewType.viewWertpapierUebersicht });
            }
                
            Messenger.Default.Send<CloseViewMessage>(new CloseViewMessage(), "StartingProgramm");
        }

        public ICommand CheckServerIsOnlineCommand { get; private set; }

        public  void CheckServerIsOnline()
        {
            using TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect("127.0.0.1", 5001);
                GlobalVariables.ServerIsOnline = true;
            }
            catch (Exception e)
            {
                GlobalVariables.ServerIsOnline = false;
                SendExceptionMessage("Backend ist nicht erreichbar" + Environment.NewLine + e.Message);
            }
        }
    }
}
