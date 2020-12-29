using Data.API;
using Data.Types;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.DividendeMessages;
using Logic.Models.DividendeModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Logic.UI.DividendeModels
{
    public class DividendenUebersichtViewModel : ViewModelBasis
    {
        private readonly ObservableCollection<Dividende> dividenden;

        private Dividende selectedDividende;

        private int aktieID;

        public DividendenUebersichtViewModel()
        {
            Messenger.Default.Register<LoadDividendeFuerAktieMessage>(this, m => ReceiveOLoadDividendeFuerAktieMessages(m));
            dividenden = new ObservableCollection<Dividende>();
            NeuCommand = new RelayCommand(() => ExecuteNeuCommand());
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
        }

        private void ReceiveOLoadDividendeFuerAktieMessages(LoadDividendeFuerAktieMessage m)
        {
            dividenden.Clear();
            aktieID = m.AktieID;
            foreach (var item in new DividendeAPI().LadeAlleFuerAktie(m.AktieID))
            {
                var ModelItem = new Dividende
                {
                    AktienID = item.Aktie.ID,
                    Aktienname = item.Aktie.Name,
                    Betrag = item.Betrag,
                    Datum = item.Datum,
                    ID = item.ID
                };
                dividenden.Add(ModelItem);
            }
            this.RaisePropertyChanged();
        }

        private void ExecuteNeuCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenNeuMessage>(new OpenDividendeStammdatenNeuMessage { AktieID = aktieID, State = State.Neu });
        }

        private void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenNeuMessage>(new OpenDividendeStammdatenNeuMessage { AktieID = aktieID, State = State.Bearbeiten, DividendeID = selectedDividende.ID });
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
            }
        }
        public IEnumerable<Dividende> Dividenden
        {
            get
            {
                return dividenden;
            }
        }
        public ICommand NeuCommand { get; protected set; }
        public ICommand BearbeitenCommand { get; protected set; }
    }
}
