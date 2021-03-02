using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DerivateMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.UI.BaseViewModels;
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

namespace Aktien.Logic.UI.DerivateViewModels
{
    public class DerivateGesamtUebersichtViewModel : ViewModelUebersicht<Wertpapier>
    {

        public DerivateGesamtUebersichtViewModel()
        {
            Title = "Übersicht aller Derivate";
            LoadData();
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanExecuteCommand);
            NeuCommand = new RelayCommand(this.ExecuteAddAktieCommand);
            RegisterAktualisereViewMessage(ViewType.viewDerivateUebersicht);
        }

        public override void LoadData()
        {
            itemList = new DerivateAPI().LadeAlle();
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
            Messenger.Default.Send<OpenDerivateStammdatenMessage>(new OpenDerivateStammdatenMessage { WertpapierID = selectedItem.ID, State = Data.Types.State.Bearbeiten });
        }


        private void ExecuteEntfernenCommand()
        {
            try
            {
                new EtfAPI().Entfernen(selectedItem);
            }
            catch (WertpapierInDepotVorhandenException)
            {
                SendExceptionMessage("Derivate ist im Depot vorhanden.");
                return;
            }

            Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = selectedItem.WertpapierTyp }, messageToken);
            itemList.Remove(SelectedItem);
            this.RaisePropertyChanged("ItemList");
            Messenger.Default.Send<DeleteDerivateErfolgreichMessage>(new DeleteDerivateErfolgreichMessage());
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewWertpapierUebersicht);
        }

        private void ExecuteAddAktieCommand()
        {
            Messenger.Default.Send<OpenDerivateStammdatenMessage>(new OpenDerivateStammdatenMessage { State = Data.Types.State.Neu });
        }
        #endregion
    }
}
