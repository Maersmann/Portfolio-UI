using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelUebersicht : ViewModelBasis
    {
        public void RegisterAktualisereViewMessage( ViewType inViewType )
        {
            Messenger.Default.Register<AktualisiereViewMessage>(this, inViewType, m => ReceiveAktualisiereViewMessage());
        }

        private void ReceiveAktualisiereViewMessage()
        {
            LoadData();
        }

        public virtual void LoadData() { }
    }
}
