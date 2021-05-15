using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelLoadData : ViewModelBasis
    {
        private StammdatenTypes typ;

        public void RegisterAktualisereViewMessage(StammdatenTypes stammdatenTypes)
        {
            typ = stammdatenTypes;
            Messenger.Default.Register<AktualisiereViewMessage>(this, stammdatenTypes, m => ReceiveAktualisiereViewMessage(m));
            
        }

        private void ReceiveAktualisiereViewMessage(AktualisiereViewMessage m)
        {
            if (m.ID.HasValue)
                LoadData(m.ID.Value);
            else
                LoadData();
        }

        public virtual void LoadData() {  this.RaisePropertyChanged("ItemList"); }
        public virtual void LoadData(int id) { this.RaisePropertyChanged("ItemList"); }

        public override void Cleanup()
        {
            Messenger.Default.Unregister<AktualisiereViewMessage>(this, typ);
            base.Cleanup();
        }




    }
}
