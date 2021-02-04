using Aktien.Data.Types;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Aktie;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.UI.BaseViewModels;
using System;
using System.Windows.Input;
using Aktien.Logic.Messages;

namespace Aktien.Logic.UI
{
    public class MainViewModel : ViewModelBasis
    {
        public MainViewModel()
        {
            Title = "Aktienübersicht";
            OpenConnectionCommand = new RelayCommand(() => ExecuteOpenConnectionCommand());
            OpenAktienUebersichtCommand = new RelayCommand(() => ExecuteOpenAktienUebersichtCommand());
            OpenDepotUebersichtCommand = new RelayCommand(() => ExecuteOpenDepotUebersichtCommand());
            OpenETFUebersichtCommand = new RelayCommand(() => ExecuteOpenETFUebersichtCommand());
        }

        public ICommand OpenAktienUebersichtCommand { get; private set; }
        public ICommand OpenETFUebersichtCommand { get; private set; }
        public ICommand OpenDepotUebersichtCommand { get; private set; }
        public ICommand OpenConnectionCommand { get; private set; }



        private void ExecuteOpenAktienUebersichtCommand()
        {
            Messenger.Default.Send<OpenViewMessage>(new OpenViewMessage { ViewType = ViewType.viewAktienUebersicht  });
        }

        private void ExecuteOpenDepotUebersichtCommand()
        {
            Messenger.Default.Send<OpenViewMessage>(new OpenViewMessage { ViewType = ViewType.viewDepotUebersicht });
        }

        private void ExecuteOpenETFUebersichtCommand()
        {
            Messenger.Default.Send<OpenViewMessage>(new OpenViewMessage { ViewType = ViewType.viewETFUebersicht });
        }

        private void ExecuteOpenConnectionCommand()
        {
            var db = new DatabaseAPI();
            db.AktualisereDatenbank();
        }

    }
}