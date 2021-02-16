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
            LoadData();
            messageToken = "";
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanExecuteCommand);
            AddAktieCommand = new RelayCommand(this.ExecuteAddAktieCommand);
            RegisterAktualisereViewMessage(ViewType.viewDerivateUebersicht);
        }

        public string MessageToken { set { messageToken = value; } }

        public override void LoadData()
        {
            itemList = new DerivateAPI().LadeAlle();
            this.RaisePropertyChanged("ItemList");
        }

        #region Binding
        public override Wertpapier SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                this.RaisePropertyChanged();
                ((DelegateCommand)BearbeitenCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)EntfernenCommand).RaiseCanExecuteChanged();
                if (selectedItem != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedItem.ID }, messageToken);
                }
            }
        }

        public ICommand BearbeitenCommand { get; protected set; }
        public ICommand EntfernenCommand { get; protected set; }
        public ICommand AddAktieCommand { get; set; }
        #endregion

        #region Commands
        private bool CanExecuteCommand()
        {
            return selectedItem != null;
        }

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

            Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = 0 });
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
