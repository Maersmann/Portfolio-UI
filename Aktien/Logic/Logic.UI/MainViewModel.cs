using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
using System.Windows.Input;

namespace Logic.UI
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Title = "Aktienübersicht";
            Button1Command = new RelayCommand(() => ExecuteButtonCommand());
        }

        private void ExecuteButtonCommand()
        {
            Messenger.Default.Send<OpenNeueAktieViewMessage>(new OpenNeueAktieViewMessage { });
        }

        public string Title { get; private set; }

        public ICommand Button1Command { get; private set; }
    }
}