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
    public class ViewModelLoadData : ViewModelBasis
    {
        public void RegisterAktualisereViewMessage(ViewType viewType)
        {
            Messenger.Default.Register<AktualisiereViewMessage>(this, viewType, m => ReceiveAktualisiereViewMessage(m));
        }

        private void ReceiveAktualisiereViewMessage(AktualisiereViewMessage m)
        {
            if (m.ID.HasValue)
                LoadData(m.ID.Value);
            else
                LoadData();
        }

        public virtual void LoadData() { }
        public virtual void LoadData(int id) { }
    }
}
