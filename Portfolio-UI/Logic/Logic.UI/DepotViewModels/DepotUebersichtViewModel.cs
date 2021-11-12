using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Base.Logic.ViewModels;
using Data.Model.DepotModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class DepotUebersichtViewModel: ViewModelUebersicht<DepotGesamtUebersichtModel, StammdatenTypes>
    {

        public DepotUebersichtViewModel()
        {
            Title = "Übersicht der Aktien im Depot";
            OpenDividendeCommand = new DelegateCommand(ExecuteOpenDividendeCommandCommand, this.CanExecuteCommand);
            OpenReverseSplitCommand = new RelayCommand(() => ExecuteOpenReverseSplitCommand());
            RegisterAktualisereViewMessage(StammdatenTypes.buysell.ToString());
        }

        protected override string GetREST_API() { return $"/api/Depot"; }

        #region Bindings
        public override DepotGesamtUebersichtModel SelectedItem
        {
            get
            {
                return base.SelectedItem;
            }
            set
            {
                base.SelectedItem = value;
                ((DelegateCommand)OpenDividendeCommand).RaiseCanExecuteChanged();
                RaisePropertyChanged();
                if (SelectedItem != null)
                {
                    Messenger.Default.Send(new LoadWertpapierOrderMessage { WertpapierID = SelectedItem.WertpapierID, WertpapierTyp = SelectedItem.WertpapierTyp }, messageToken);
                }
            }
        }

        public ICommand OpenDividendeCommand { get; set; }
        public ICommand OpenReverseSplitCommand { get; set; }
        #endregion

        #region Commands
        protected override bool CanExecuteCommand()
        {
            return base.CanExecuteCommand() && (SelectedItem.WertpapierTyp.Equals(WertpapierTypes.Aktie));
        }

        private void ExecuteOpenDividendeCommandCommand()
        {
            Messenger.Default.Send(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = SelectedItem.WertpapierID }, "DepotUebersicht");
        }

        private void ExecuteOpenReverseSplitCommand()
        {
            Messenger.Default.Send(new OpenReverseSplitEintragenMessage { DepotWertpapierID = SelectedItem.WertpapierID}, "DepotUebersicht");
        }
        #endregion
    }
}
