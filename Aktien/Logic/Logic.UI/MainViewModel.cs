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

namespace Aktien.Logic.UI
{
    public class MainViewModel : ViewModelBasis
    {
        public MainViewModel()
        {
            Title = "Aktienübersicht";
            OpenConnectionCommand = new RelayCommand(() => ExecuteOpenConnectionCommand());
            OpenAktienUebersichtCommand = new RelayCommand(() => ExecuteOpenAktienUebersichtCommand());
            OpenAktieGekauftCommand = new RelayCommand(() => ExecuteOpenAktieGekauftCommand());
        }

        public ICommand OpenAktienUebersichtCommand { get; private set; }

        public ICommand OpenConnectionCommand { get; private set; }

        public ICommand OpenAktieGekauftCommand { get; private set; }


        private void ExecuteOpenAktienUebersichtCommand()
        {
            Messenger.Default.Send<OpenViewMessage>(new OpenViewMessage { ViewType = ViewType.viewAktienUebersicht  });
        }

        private void ExecuteOpenAktieGekauftCommand()
        {
            Messenger.Default.Send<OpenViewMessage>(new OpenViewMessage { ViewType = ViewType.viewAktieGekauft });
        }

        private void ExecuteOpenConnectionCommand()
        {
            var db = new DatabaseAPI();
            db.AktualisereDatenbank();
        }

    }
}