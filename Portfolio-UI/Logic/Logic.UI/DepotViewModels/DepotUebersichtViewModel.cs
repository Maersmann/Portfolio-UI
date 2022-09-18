using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Base.Logic.ViewModels;
using Data.Model.DepotModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.DepotMessages;
using Logic.Messages.WertpapierMessages;
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
    public class DepotUebersichtViewModel : ViewModelUebersicht<DepotGesamtUebersichtModel, StammdatenTypes>
    {

        public DepotUebersichtViewModel()
        {
            Title = "Übersicht der Aktien im Depot";
            OpenDividendeCommand = new DelegateCommand(ExecuteOpenDividendeCommandCommand, CanExecuteCommand);
            OpenReverseSplitCommand = new RelayCommand(() => ExecuteOpenReverseSplitCommand());
            OpenAktienSplitCommand = new RelayCommand(() => ExecuteOpenAktienSplitCommand());
            OpenErhaltendeDividendeEintragenCommand = new RelayCommand(() => ExecuteOpenErhaltendeDividendeEintragenCommand());
            RegisterAktualisereViewMessage(StammdatenTypes.buysell.ToString());
        }

        protected override string GetREST_API() { return $"/api/Depot"; }
        protected override bool WithPagination() { return true; }

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
        public ICommand OpenAktienSplitCommand { get; set; }
        public ICommand OpenErhaltendeDividendeEintragenCommand { get; set; }
        #endregion

        #region Commands
        protected override bool CanExecuteCommand()
        {
            return base.CanExecuteCommand() && SelectedItem.WertpapierTyp.Equals(WertpapierTypes.Aktie);
        }

        private void ExecuteOpenErhaltendeDividendeEintragenCommand()
        {
            Messenger.Default.Send(new OpenErhalteneDividendeEintragenMessage { WertpapierID = SelectedItem.WertpapierID, WertpapierName = SelectedItem.Bezeichnung }, "DepotUebersicht");
        }

        private void ExecuteOpenDividendeCommandCommand()
        {
            Messenger.Default.Send(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = SelectedItem.WertpapierID }, "DepotUebersicht");
        }

        private void ExecuteOpenReverseSplitCommand()
        {
            Messenger.Default.Send(new OpenReverseSplitEintragenMessage { DepotWertpapierID = SelectedItem.WertpapierID}, "DepotUebersicht");
        }

        private void ExecuteOpenAktienSplitCommand()
        {
            Messenger.Default.Send(new OpenSplitEintragenMessage { DepotWertpapierID = SelectedItem.WertpapierID }, "DepotUebersicht");
        }
        #endregion
    }
}
