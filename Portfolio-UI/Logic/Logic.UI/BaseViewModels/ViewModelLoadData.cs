using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelLoadData<T> : ViewModelBasis
    {
        protected string filtertext;
        private StammdatenTypes typ;
        protected ICollectionView _customerView;
        protected ObservableCollection<T> itemList;
        protected T selectedItem;

        public void RegisterAktualisereViewMessage(StammdatenTypes stammdatenTypes)
        {
            typ = stammdatenTypes;
            Messenger.Default.Register<AktualisiereViewMessage>(this, stammdatenTypes, m => ReceiveAktualisiereViewMessage(m));
            itemList = new ObservableCollection<T>();
            _customerView = CollectionViewSource.GetDefaultView(ItemList);
            _customerView.Filter = OnFilterTriggered;
            filtertext = "";
        }

        private void ReceiveAktualisiereViewMessage(AktualisiereViewMessage m)
        {
            if (m.ID.HasValue)
                LoadData(m.ID.Value);
            else
                LoadData();
        }

        public virtual void LoadData()
        {
            _customerView = (CollectionView)CollectionViewSource.GetDefaultView(itemList);
            _customerView.Filter = OnFilterTriggered;
            RaisePropertyChanged("ItemList");
            RaisePropertyChanged("ItemCollection");
        }
        public virtual void LoadData(int id)
        {
            _customerView = (CollectionView)CollectionViewSource.GetDefaultView(itemList);
            _customerView.Filter = OnFilterTriggered;
            RaisePropertyChanged("ItemList");
            RaisePropertyChanged("ItemCollection");
        }

        protected virtual bool OnFilterTriggered(object item)
        {
            return true;
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
            }
        }
        public IEnumerable<T> ItemList
        {
            get
            {
                return itemList;
            }
        }
        public ICollectionView ItemCollection { get { return _customerView; } }

        public override void Cleanup()
        {
            Messenger.Default.Unregister<AktualisiereViewMessage>(this, typ);
            base.Cleanup();
        }




    }
}
