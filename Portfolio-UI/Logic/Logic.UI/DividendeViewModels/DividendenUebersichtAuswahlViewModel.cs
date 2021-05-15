﻿using Aktien.Logic.Messages.DividendeMessages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendenUebersichtAuswahlViewModel
    {
        public DividendenUebersichtAuswahlViewModel()
        { 
            OpenAlleDividendenViewCommand = new RelayCommand(this.ExecuteOpenAlleDividendenViewCommand);
            OpenErhalteneDividendenViewCommand = new RelayCommand(this.ExecuteOpenErhalteneDividendenViewCommand);
        }

        private void ExecuteOpenErhalteneDividendenViewCommand()
        {
            Messenger.Default.Send<OpenDividendeErhaltenUebersichtViewMessage>(new OpenDividendeErhaltenUebersichtViewMessage { WertpapierID = WertpapierID });
        }

        private void ExecuteOpenAlleDividendenViewCommand()
        {
            Messenger.Default.Send<OpenDividendeUebersichtMessage>(new OpenDividendeUebersichtMessage { WertpapierID = WertpapierID });
        }

        public ICommand OpenAlleDividendenViewCommand { get; set; }
        public ICommand OpenErhalteneDividendenViewCommand { get; set; }

        public int WertpapierID { get; set; }
    }
}
