using Aktien.Data.Model.AktienModels;
using Aktien.Data.Types;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.DividendeMessages;
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

namespace Aktien.Logic.UI.AuswahlViewModels
{
    public class DividendenAuswahlViewModel : ViewModelBasis
    {
        private int aktieID;
        private ObservableCollection<Dividende> dividenden;

        private Dividende selectedDividende;

        public DividendenAuswahlViewModel()
        {
            dividenden = new ObservableCollection<Dividende>();
            AuswahlCommand = new DelegateCommand(this.ExecutAuswahlCommand, this.CanExecuteCommand);
            AddCommand = new RelayCommand(this.ExcecuteAddCommand);
        }

        private void ExcecuteAddCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenMessage>(new OpenDividendeStammdatenMessage { AktieID = aktieID, State = State.Neu });
        }

        private bool CanExecuteCommand()
        {
            return selectedDividende != null;
        }

        private void ExecutAuswahlCommand()
        {
            Messenger.Default.Send<DividendeAusgewaehltMessage>(new DividendeAusgewaehltMessage {  ID = selectedDividende.ID, Betrag = selectedDividende.Betrag, Datum = selectedDividende.Datum});
        }

        #region Bindings
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
                ((DelegateCommand)AuswahlCommand).RaiseCanExecuteChanged();
            }
        }
        public IEnumerable<Dividende> Dividenden
        {
            get
            {
                return dividenden;
            }
        }
        public ICommand AuswahlCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public void LoadData(int inAktieID)
        {
            aktieID = inAktieID;
            dividenden = new DividendeAPI().LadeAlleFuerAktie(aktieID);
            this.RaisePropertyChanged("Dividenden");
        }
        #endregion
    }
}
