using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelUebersicht<T> : ViewModelLoadData
    {
        protected ObservableCollection<T> itemList;

        protected T selectedItem;

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
            }
        }

        public IEnumerable<T> ItemList
        {
            get
            {
                return itemList;
            }
        }
    }
}
