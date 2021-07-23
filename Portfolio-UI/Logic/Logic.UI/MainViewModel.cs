using Aktien.Data.Types;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Core;
using Aktien.Logic.UI.BaseViewModels;
using System;
using System.Windows.Input;
using Aktien.Logic.Messages;
using Aktien.Logic.Messages.Base;
using System.Net.Sockets;
using Logic.UI.Helper;
using Logic.Core.OptionenLogic;
using Logic.Messages.Base;

namespace Aktien.Logic.UI
{
    public class MainViewModel : ViewModelBasis
    {
        public MainViewModel()
        {
            Title = "Aktienübersicht";
            GlobalVariables.ServerIsOnline = false;
            GlobalVariables.BackendServer_URL = "";
            OpenStartingViewCommand = new RelayCommand(() => ExecuteOpenStartingViewCommand());
            OpenAktienUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand( ViewType.viewAktienUebersicht));
            OpenDepotUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewDepotUebersicht));
            OpenETFUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewETFUebersicht));
            OpenWertpapierUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewWertpapierUebersicht));
            OpenDerivateUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewDerivateUebersicht));
            OpenEinAusgabenUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewEinAusgabenUebersicht));
            OpenDivideneMonatAuswertungCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewDivideneMonatAuswertung));



        }

        public ICommand OpenAktienUebersichtCommand { get; private set; }
        public ICommand OpenETFUebersichtCommand { get; private set; }
        public ICommand OpenDepotUebersichtCommand { get; private set; }
        public ICommand OpenWertpapierUebersichtCommand { get; private set; }
        public ICommand OpenDerivateUebersichtCommand { get; private set; }
        public ICommand OpenEinAusgabenUebersichtCommand { get; private set; }
        public ICommand OpenStartingViewCommand { get; private set; }
        public ICommand OpenDivideneMonatAuswertungCommand { get; private set; }

        public bool MenuIsEnabled => GlobalVariables.ServerIsOnline;

        private void ExecuteOpenViewCommand(ViewType viewType)
        {
            Messenger.Default.Send<OpenViewMessage>(new OpenViewMessage { ViewType = viewType });
        }
        private void ExecuteOpenStartingViewCommand()
        {
            var backendlogic = new BackendLogic();
            if(!backendlogic.istINIVorhanden())
            {
                Messenger.Default.Send<OpenKonfigurationViewMessage>(new OpenKonfigurationViewMessage { });
            }
            backendlogic.LoadData();
            GlobalVariables.BackendServer_IP = backendlogic.getBackendIP();
            GlobalVariables.BackendServer_URL = backendlogic.getURL();
            GlobalVariables.BackendServer_Port = backendlogic.getBackendPort();

            Messenger.Default.Send<OpenStartingViewMessage>(new OpenStartingViewMessage { });
        }

    }
}