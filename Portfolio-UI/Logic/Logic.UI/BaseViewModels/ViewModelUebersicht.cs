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
    public class ViewModelUebersicht<T> : ViewModelLoadData<T>
    {

        public ViewModelUebersicht()
        {
            itemList = new ObservableCollection<T>();
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanExecuteCommand);
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
            NeuCommand = new RelayCommand(() => ExecuteNeuCommand());
        }

        protected virtual int GetID() { return 0; }
        protected virtual StammdatenTypes GetStammdatenType() { return 0; }

        public override T SelectedItem
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

        public string FilterText
        {
            get => filtertext;
            set
            {
                filtertext = value;
                RaisePropertyChanged();
                _customerView.Refresh();
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
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), GetStammdatenType());
        }

        protected virtual void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<BaseStammdatenMessage>(new BaseStammdatenMessage { State = State.Bearbeiten, ID = GetID(), StammdatenTyp = GetStammdatenType() });
        }

        protected virtual void ExecuteNeuCommand()
        {
            Messenger.Default.Send<BaseStammdatenMessage>(new BaseStammdatenMessage { State = State.Neu, ID = null, StammdatenTyp = GetStammdatenType() });
        }

        public  ICommand NeuCommand { get; protected set; }
        public  ICommand BearbeitenCommand { get; protected set; }
        public  ICommand EntfernenCommand { get; protected set; }
    }
}
