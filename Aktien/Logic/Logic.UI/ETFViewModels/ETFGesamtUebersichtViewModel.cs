using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.ETFViewModels
{
    public class ETFGesamtUebersichtViewModel : ViewModelUebersicht<Wertpapier>
    {

        public ETFGesamtUebersichtViewModel()
        {
            Title = "Übersicht aller ETF's";
            LoadData();
            RegisterAktualisereViewMessage(StammdatenTypes.etf);
        }
        protected override int GetID() { return selectedItem.ID; }
        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.etf ; }


        public override void LoadData()
        {
            itemList = new EtfAPI().LadeAlle();
            this.RaisePropertyChanged("ItemList");
        }

        #region Binding
        public override Wertpapier SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                base.SelectedItem = value;
                if (selectedItem != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedItem.ID, WertpapierTyp = selectedItem.WertpapierTyp }, messageToken);
                }
            }
        }
        #endregion

        #region Commands


        protected override void ExecuteEntfernenCommand()
        {
            try
            {
                new EtfAPI().Entfernen(selectedItem);
            }
            catch (WertpapierInDepotVorhandenException)
            {
                SendExceptionMessage("ETF ist im Depot vorhanden.");
                return;
            }

            Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = selectedItem.WertpapierTyp }, messageToken);
            itemList.Remove(selectedItem);
            this.RaisePropertyChanged("ItemList");
            SendInformationMessage("ETF gelöscht");
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewWertpapierUebersicht);

        }

        #endregion
    }
}
