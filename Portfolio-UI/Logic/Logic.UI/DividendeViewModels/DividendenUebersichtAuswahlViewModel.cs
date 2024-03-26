using Aktien.Logic.Messages.DividendeMessages;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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
             WeakReferenceMessenger.Default.Send(new OpenDividendeErhaltenUebersichtViewMessage { WertpapierID = WertpapierID });
        }

        private void ExecuteOpenAlleDividendenViewCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenDividendeUebersichtMessage { WertpapierID = WertpapierID });
        }

        public ICommand OpenAlleDividendenViewCommand { get; set; }
        public ICommand OpenErhalteneDividendenViewCommand { get; set; }

        public int WertpapierID { get; set; }
    }
}
