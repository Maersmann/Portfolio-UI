using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
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
        }

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

        public  ICommand NeuCommand { get; protected set; }
        public  ICommand BearbeitenCommand { get; protected set; }
        public  ICommand EntfernenCommand { get; protected set; }
    }
}
