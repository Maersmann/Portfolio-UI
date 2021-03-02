using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.ETFMessages;
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
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanExecuteCommand);
            NeuCommand = new RelayCommand(this.ExecuteAddAktieCommand);
        }

       
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

        private void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenETFStammdatenMessage>(new OpenETFStammdatenMessage { WertpapierID = selectedItem.ID, State = Data.Types.State.Bearbeiten });
        }

        private void ExecuteEntfernenCommand()
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
            Messenger.Default.Send<DeleteEtfErfolgreichMessage>(new DeleteEtfErfolgreichMessage());
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewWertpapierUebersicht);

        }

        private void ExecuteAddAktieCommand()
        {
            Messenger.Default.Send<OpenETFStammdatenMessage>(new OpenETFStammdatenMessage { State = Data.Types.State.Neu });
        }
        #endregion
    }
}
