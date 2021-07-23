using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelAuswahl<T> : ViewModelLoadData<T>
    {
        public ViewModelAuswahl()
        {
            itemList = new ObservableCollection<T>();
            NewItemCommand = new RelayCommand(this.ExcecuteNewItemCommand);
        }


        protected virtual void ExcecuteNewItemCommand()
        {
            Messenger.Default.Send<BaseStammdatenMessage>(new BaseStammdatenMessage { State = State.Neu, ID = null, StammdatenTyp = GetStammdatenType() });
        }

        public ICommand NewItemCommand { get; set; }

        protected virtual StammdatenTypes GetStammdatenType() { return 0; }
    }
}
