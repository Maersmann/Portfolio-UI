using Data.API;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
using Logic.Messages.AktieMessages;
using System;
using System.Windows.Input;

namespace Logic.UI
{
    public class MainViewModel : ViewModelBasis
    {
        public MainViewModel()
        {
            Title = "Aktienübersicht";
            OpenNeueAktieCommand = new RelayCommand(() => ExecuteOpenNeueAktieCommand());
            OpenConnectionCommand = new RelayCommand(() => ExecuteOpenConnectionCommand());
            OpenAktienUebersichtCommand = new RelayCommand(() => ExecuteOpenAktienUebersichtCommand());
        }

        public ICommand OpenNeueAktieCommand { get; private set; }

        public ICommand OpenAktienUebersichtCommand { get; private set; }

        public ICommand OpenConnectionCommand { get; private set; }

        private void ExecuteOpenNeueAktieCommand()
        {
            Messenger.Default.Send<OpenAktieStammdatenMessage>(new OpenAktieStammdatenMessage { });
        }

        private void ExecuteOpenAktienUebersichtCommand()
        {
            Messenger.Default.Send<OpenAuswahlAktieMessage>(new OpenAuswahlAktieMessage { });
        }

        private void ExecuteOpenConnectionCommand()
        {
            DatabaseAPI dbAPI = new DatabaseAPI();
            dbAPI.OpenConnection();
        }

    }
}