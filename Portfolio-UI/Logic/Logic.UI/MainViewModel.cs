using Aktien.Data.Types;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Core;
using Base.Logic.ViewModels;
using System;
using System.Windows.Input;
using Aktien.Logic.Messages;
using Aktien.Logic.Messages.Base;
using System.Net.Sockets;
using Logic.Core.OptionenLogic;
using Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.Messages;

namespace Aktien.Logic.UI
{
    public class MainViewModel : ViewModelBasis
    {
        public MainViewModel()
        {
            Title = "Aktienübersicht";
            GlobalVariables.ServerIsOnline = false;
            GlobalVariables.BackendServer_URL = "";
            GlobalVariables.Token = "";
            OpenStartingViewCommand = new RelayCommand(() => ExecuteOpenStartingViewCommand());
            OpenAktienUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand( ViewType.viewAktienUebersicht));
            OpenDepotUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewDepotUebersicht));
            OpenETFUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewETFUebersicht));
            OpenWertpapierUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewWertpapierUebersicht));
            OpenDerivateUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewDerivateUebersicht));
            OpenEinAusgabenUebersichtCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewEinAusgabenUebersicht));
            OpenDivideneMonatAuswertungCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewDivideneMonatAuswertung));
            OpenDivideneMonatJahresvergleichAuswertungCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewDivideneMonatJahresauswertungAuswertung));
            OpenSteuerartMonatAuswertungCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewSteuerartMonatAuswertung));
            OpenSteuerMonatJahresvergleichAuswertungCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewSteuerMonatJahresAuswertung));
            OpenDivideneWertpapierAuswertungCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewDivideneWertpapierAuswertung));
            OpenSteuerMonatAuswertungCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewSteuerMonatAuswertung));
            OpenDividendeWertpapierEntwicklungAuswertungCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewOpenDividendeWertpapierEntwicklungAuswertung));
            OpenDividendenErhaltenImJahrCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewOpenDividendenErhaltenImJahr));
            OpenDividendenErhaltenImMonatCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewOpenDividendenErhaltenImMonat));
            OpenOrderBuchCommand = new RelayCommand(() => ExecuteOpenViewCommand(ViewType.viewOpenOrderBuch));


            Messenger.Default.Register<AktualisiereBerechtigungenMessage>(this, m => ReceiveOpenViewMessage());
        }

        public ICommand OpenAktienUebersichtCommand { get; private set; }
        public ICommand OpenETFUebersichtCommand { get; private set; }
        public ICommand OpenDepotUebersichtCommand { get; private set; }
        public ICommand OpenWertpapierUebersichtCommand { get; private set; }
        public ICommand OpenDerivateUebersichtCommand { get; private set; }
        public ICommand OpenEinAusgabenUebersichtCommand { get; private set; }
        public ICommand OpenStartingViewCommand { get; private set; }
        public ICommand OpenDivideneMonatAuswertungCommand { get; private set; }
        public ICommand OpenDivideneMonatJahresvergleichAuswertungCommand { get; private set; }
        public ICommand OpenSteuerartMonatAuswertungCommand { get; private set; }
        public ICommand OpenSteuerMonatJahresvergleichAuswertungCommand { get; private set; }
        public ICommand OpenDivideneWertpapierAuswertungCommand { get; private set; }
        public ICommand OpenSteuerMonatAuswertungCommand { get; private set; }
        public ICommand OpenDividendeWertpapierEntwicklungAuswertungCommand { get; private set; }
        public ICommand OpenDividendenErhaltenImMonatCommand { get; private set; }
        public ICommand OpenDividendenErhaltenImJahrCommand { get; private set; }
        public ICommand OpenOrderBuchCommand { get; private set; }
        public bool MenuIsEnabled => GlobalVariables.ServerIsOnline;

        private void ExecuteOpenViewCommand(ViewType viewType)
        {
            Messenger.Default.Send(new OpenViewMessage { ViewType = viewType });
        }
        private void ExecuteOpenStartingViewCommand()
        {
            BackendLogic backendlogic = new BackendLogic();
            if(!backendlogic.istINIVorhanden())
            {
                Messenger.Default.Send(new OpenKonfigurationViewMessage { });
            }
            backendlogic.LoadData();
            GlobalVariables.BackendServer_IP = backendlogic.getBackendIP();
            GlobalVariables.BackendServer_URL = backendlogic.getURL();
            GlobalVariables.BackendServer_Port = backendlogic.getBackendPort();

            Messenger.Default.Send(new OpenStartingViewMessage { });
        }

        private void ReceiveOpenViewMessage()
        {
            RaisePropertyChanged("MenuIsEnabled");
        }

    }
}