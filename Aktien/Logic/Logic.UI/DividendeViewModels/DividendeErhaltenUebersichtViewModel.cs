using Aktien.Data.Model.AktienModels;
using Aktien.Data.Types;
using Aktien.Logic.Core.AktieLogic;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight;
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

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeErhaltenUebersichtViewModel : ViewModelBasis
    {
        private ObservableCollection<DividendeErhalten> dividenden;

        private DividendeErhalten selectedDividende;

        private int aktieID;

        public DividendeErhaltenUebersichtViewModel()
        {
            dividenden = new ObservableCollection<DividendeErhalten>();
            NeuCommand = new RelayCommand(() => ExecuteNeuCommand());
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanExecuteCommand);
        }

        public void LoadData(int inAktieID)
        {
            aktieID = inAktieID;
            dividenden = new AktieAPI().LadeAlleErhalteneDividenden(aktieID);
            this.RaisePropertyChanged("Dividenden");
        }

        #region Commands
        private void ExecuteNeuCommand()
        {
            Messenger.Default.Send<OpenErhaltendeDividendeStammdatenMessage>(new OpenErhaltendeDividendeStammdatenMessage { AktieID = aktieID, State = State.Neu });
        }

        private bool CanExecuteCommand()
        {
            return selectedDividende != null;
        }

        private void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenErhaltendeDividendeStammdatenMessage>(new OpenErhaltendeDividendeStammdatenMessage { AktieID = aktieID, State = State.Bearbeiten, ID = selectedDividende.ID });
        }

        private void ExecuteEntfernenCommand()
        {
            new DividendeErhaltenAPI().Entfernen(selectedDividende.ID);
            dividenden.Remove(selectedDividende);
            this.RaisePropertyChanged("Dividenden");
            Messenger.Default.Send<DeleteErhalteneDividendenErfolgreichMessage>(new DeleteErhalteneDividendenErfolgreichMessage());
        }
        #endregion


        #region Binding
        public DividendeErhalten SelectedDividende
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
        public IEnumerable<DividendeErhalten> Dividenden
        {
            get
            {
                return dividenden;
            }
        }
        public ICommand NeuCommand { get; private set; }
        public ICommand BearbeitenCommand { get; set; }

        public ICommand EntfernenCommand { get; set; }
        #endregion
    }
}
