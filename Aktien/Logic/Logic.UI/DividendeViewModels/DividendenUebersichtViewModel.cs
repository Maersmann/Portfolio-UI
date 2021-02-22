using Aktien.Data.Types;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.BaseViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Aktien.Data.Model.WertpapierEntitys;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendenUebersichtViewModel : ViewModelUebersicht<Dividende>
    {

        private int wertpapierID;

        public DividendenUebersichtViewModel()
        {
            Title = "Übersicht aller Dividenden";
            itemList = new ObservableCollection<Dividende>();
            NeuCommand = new RelayCommand(() => ExecuteNeuCommand());
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanExecuteCommand);
            RegisterAktualisereViewMessage(ViewType.viewDividendeUebersicht);
        }

        #region Commands
        private void ExecuteNeuCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenMessage>(new OpenDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Neu });
        }
        private void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenMessage>(new OpenDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Bearbeiten, DividendeID = selectedItem.ID });
        }

        public override void LoadData(int id)
        {
            wertpapierID = id;
            itemList = new DividendeAPI().LadeAlleFuerWertpapier(wertpapierID);
            this.RaisePropertyChanged("ItemList");
        }

        private void ExecuteEntfernenCommand()
        {
            new DividendeAPI().Entfernen(selectedItem.ID);
            itemList.Remove(selectedItem);
            this.RaisePropertyChanged("ItemList");
            Messenger.Default.Send<DeleteDividendeErfolgreichMessage>(new DeleteDividendeErfolgreichMessage() );
        }
        private bool CanExecuteCommand()
        {
            return selectedItem != null;
        }
        #endregion

        #region Bindigs
        public override Dividende SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                this.RaisePropertyChanged();
                ((DelegateCommand)BearbeitenCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)EntfernenCommand).RaiseCanExecuteChanged();
            }
        }
        public ICommand NeuCommand { get; private set; }
        public ICommand BearbeitenCommand { get; private set; }
        public ICommand EntfernenCommand { get; private set; }
        #endregion
    }
}
