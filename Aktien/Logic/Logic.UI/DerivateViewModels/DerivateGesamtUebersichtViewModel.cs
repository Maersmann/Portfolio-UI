using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;
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
    public class DerivateGesamtUebersichtViewModel : ViewModelUebersicht
    {
        private ObservableCollection<Wertpapier> alleDerivate;

        private Wertpapier selectedDerivate;

        public DerivateGesamtUebersichtViewModel()
        {
            LoadData();
            messageToken = "";
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanExecuteCommand);
            AddAktieCommand = new RelayCommand(this.ExecuteAddAktieCommand);
        }

        public string MessageToken { set { messageToken = value; } }

        public override void LoadData()
        {
            alleDerivate = new DerivateAPI().LadeAlle();
            this.RaisePropertyChanged("AlleDerivate");
        }

        #region Binding
        public Wertpapier SelectedDerivate
        {
            get
            {
                return selectedDerivate;
            }
            set
            {
                selectedDerivate = value;
                this.RaisePropertyChanged();
                ((DelegateCommand)BearbeitenCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)EntfernenCommand).RaiseCanExecuteChanged();
                if (selectedDerivate != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedDerivate.ID }, messageToken);
                }
            }
        }
        public IEnumerable<Wertpapier> AlleDerivate
        {
            get
            {
                return alleDerivate;
            }
        }
        public ICommand BearbeitenCommand { get; protected set; }
        public ICommand EntfernenCommand { get; protected set; }
        public ICommand AddAktieCommand { get; set; }
        #endregion

        #region Commands
        private bool CanExecuteCommand()
        {
            return selectedDerivate != null;
        }

        private void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenDerivateStammdatenMessage>(new OpenDerivateStammdatenMessage { WertpapierID = selectedDerivate.ID, State = Data.Types.State.Bearbeiten });
        }


        private void ExecuteEntfernenCommand()
        {
            try
            {
                new EtfAPI().Entfernen(selectedDerivate);
            }
            catch (WertpapierInDepotVorhandenException)
            {
                SendExceptionMessage("Derivate ist im Depot vorhanden.");
                return;
            }

            Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = 0 });
            alleDerivate.Remove(SelectedDerivate);
            this.RaisePropertyChanged("AlleAktien");
            Messenger.Default.Send<DeleteDerivateErfolgreichMessage>(new DeleteDerivateErfolgreichMessage());

        }

        private void ExecuteAddAktieCommand()
        {
            Messenger.Default.Send<OpenDerivateStammdatenMessage>(new OpenDerivateStammdatenMessage { State = Data.Types.State.Neu });
        }
        #endregion
    }
}
