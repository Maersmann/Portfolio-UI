using Aktien.Logic.Messages.DividendeMessages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendenUebersichtAuswahlViewModel
    {
        public DividendenUebersichtAuswahlViewModel()
        { 
            OpenAlleDividendenViewCommand = new RelayCommand(ExecuteOpenAlleDividendenViewCommand);
            OpenErhalteneDividendenViewCommand = new RelayCommand(ExecuteOpenErhalteneDividendenViewCommand);
        }

        private void ExecuteOpenErhalteneDividendenViewCommand()
        {
            Messenger.Default.Send(new OpenDividendeErhaltenUebersichtViewMessage { WertpapierID = WertpapierID });
        }

        private void ExecuteOpenAlleDividendenViewCommand()
        {
            Messenger.Default.Send(new OpenDividendeUebersichtMessage { WertpapierID = WertpapierID });
        }

        public ICommand OpenAlleDividendenViewCommand { get; set; }
        public ICommand OpenErhalteneDividendenViewCommand { get; set; }

        public int WertpapierID { get; set; }
    }
}
