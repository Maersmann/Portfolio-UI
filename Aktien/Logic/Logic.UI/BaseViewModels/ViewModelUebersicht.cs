using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelUebersicht<T> : ViewModelLoadData
    {
        protected ObservableCollection<T> itemList;

        protected T selectedItem;

        public ViewModelUebersicht()
        {
            itemList = new ObservableCollection<T>();
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanExecuteCommand);
            NeuCommand = new RelayCommand(() => ExecuteNeuCommand());
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
        }

        protected virtual int getID() { return 0; }
        protected virtual ViewType getVStammdatenViewType() { return 0; }

        public virtual T SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                this.RaisePropertyChanged();
                if (BearbeitenCommand != null) ((DelegateCommand)BearbeitenCommand).RaiseCanExecuteChanged();
                if (EntfernenCommand != null) ((DelegateCommand)EntfernenCommand).RaiseCanExecuteChanged();

            }
        }

        public IEnumerable<T> ItemList
        {
            get
            {
                return itemList;
            }
        }

        protected virtual bool CanExecuteCommand()
        {
            return selectedItem != null;
        }

        protected virtual void ExecuteEntfernenCommand()
        {
            itemList.Remove(selectedItem);
            this.RaisePropertyChanged("ItemList");
        }

        protected virtual void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<BaseStammdatenMessage>(new BaseStammdatenMessage { State = State.Bearbeiten, ID = getID(), ViewType = getVStammdatenViewType() });
        }

        protected virtual void ExecuteNeuCommand()
        {
            Messenger.Default.Send<BaseStammdatenMessage>(new BaseStammdatenMessage { State = State.Neu, ID = null, ViewType = getVStammdatenViewType() });
        }

        public  ICommand NeuCommand { get; protected set; }
        public  ICommand BearbeitenCommand { get; protected set; }
        public  ICommand EntfernenCommand { get; protected set; }
    }
}
