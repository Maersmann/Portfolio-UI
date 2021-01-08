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
using Aktien.Data.Model.AktieModels;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendenUebersichtViewModel : ViewModelBasis
    {
        private ObservableCollection<Dividende> dividenden;

        private Dividende selectedDividende;

        private int aktieID;

        public DividendenUebersichtViewModel()
        {
            Messenger.Default.Register<LoadDividendeFuerAktieMessage>(this, m => ReceiveOLoadDividendeFuerAktieMessages(m));
            Messenger.Default.Register<AktualisiereDividendenMessage>(this, m => ReceiveAktualisiereDividendenMessage());
            dividenden = new ObservableCollection<Dividende>();
            NeuCommand = new RelayCommand(() => ExecuteNeuCommand());
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanExecuteCommand);
        }

        private void ReceiveAktualisiereDividendenMessage()
        {
            dividenden.Clear();
            dividenden = new DividendeAPI().LadeAlleFuerAktie(aktieID);
            this.RaisePropertyChanged("Dividenden");
        }
        private void ReceiveOLoadDividendeFuerAktieMessages(LoadDividendeFuerAktieMessage m)
        {
            dividenden.Clear();
            aktieID = m.AktieID;
            dividenden = new DividendeAPI().LadeAlleFuerAktie(aktieID);
            this.RaisePropertyChanged("Dividenden");
        }
       

        private void ExecuteNeuCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenNeuMessage>(new OpenDividendeStammdatenNeuMessage { AktieID = aktieID, State = State.Neu });
        }
        private void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenNeuMessage>(new OpenDividendeStammdatenNeuMessage { AktieID = aktieID, State = State.Bearbeiten, DividendeID = selectedDividende.ID });
        }
        private void ExecuteEntfernenCommand()
        {
            new DividendeAPI().Entfernen(selectedDividende.ID);
            Messenger.Default.Send<DeleteDividendeErfolgreichMessage>(new DeleteDividendeErfolgreichMessage() );
        }
        private bool CanExecuteCommand()
        {
            return selectedDividende != null;
        }


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
    }
}
