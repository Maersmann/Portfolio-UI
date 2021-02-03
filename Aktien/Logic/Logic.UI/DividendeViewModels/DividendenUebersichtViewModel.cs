using Aktien.Data.Types;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.BaseViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Aktien.Data.Model.AktienModels;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendenUebersichtViewModel : ViewModelBasis
    {
        private ObservableCollection<Dividende> dividenden;

        private Dividende selectedDividende;

        private int aktieID;

        public DividendenUebersichtViewModel()
        {
            dividenden = new ObservableCollection<Dividende>();
            NeuCommand = new RelayCommand(() => ExecuteNeuCommand());
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanExecuteCommand);
        }

        #region Commands
        private void ExecuteNeuCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenMessage>(new OpenDividendeStammdatenMessage { AktieID = aktieID, State = State.Neu });
        }
        private void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenMessage>(new OpenDividendeStammdatenMessage { AktieID = aktieID, State = State.Bearbeiten, DividendeID = selectedDividende.ID });
        }

        public void LoadData(int inAktieID)
        {
            aktieID = inAktieID;
            dividenden = new DividendeAPI().LadeAlleFuerAktie(aktieID);
            this.RaisePropertyChanged("Dividenden");
        }

        private void ExecuteEntfernenCommand()
        {
            new DividendeAPI().Entfernen(selectedDividende.ID);
            dividenden.Remove(selectedDividende);
            this.RaisePropertyChanged("Dividenden");
            Messenger.Default.Send<DeleteDividendeErfolgreichMessage>(new DeleteDividendeErfolgreichMessage() );
        }
        private bool CanExecuteCommand()
        {
            return selectedDividende != null;
        }
        #endregion

        #region Bindigs
        public Dividende SelectedDividende
        {
            get
            {
                return selectedDividende;
            }
            set
            {
                selectedDividende = value;
                this.RaisePropertyChanged();
                ((DelegateCommand)BearbeitenCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)EntfernenCommand).RaiseCanExecuteChanged();
            }
        }
        public IEnumerable<Dividende> Dividenden
        {
            get
            {
                return dividenden;
            }
        }
        public ICommand NeuCommand { get; private set; }
        public ICommand BearbeitenCommand { get; private set; }
        public ICommand EntfernenCommand { get; private set; }
        #endregion
    }
}
