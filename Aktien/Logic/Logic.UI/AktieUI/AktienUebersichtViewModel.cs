using Data;
using Data.API;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
using Logic.Messages.AktieMessages;
using Logic.Messages.Base;
using Logic.UI.BaseModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Logic.UI.AktieUI
{
    public class AktienUebersichtViewModel : ViewModelBasis
    {
        private ObservableCollection<Aktie> alleAktien;

        public AktienUebersichtViewModel()
        {
            Title = "Alle Aktien";
            AktieAPI api = new AktieAPI();
            alleAktien = api.LadeAlle();
            Messenger.Default.Register<AktualisiereViewMessage>(this, m => ReceiveAktualisiereViewMessage());
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteBearbeitenCommand); 
        }


        private Aktie selectedAktie;
        public Aktie SelectedAktie 
        {
            get 
            {
                return selectedAktie; 
            }              
            set
            {
                selectedAktie = value;
                this.RaisePropertyChanged();
                ((DelegateCommand)BearbeitenCommand).RaiseCanExecuteChanged();
             } 
        }


        

        public IEnumerable<Aktie> AlleAktien 
        {
            get
            {
                return alleAktien;
            }
        }

        public ICommand BearbeitenCommand { get; protected set; }


        private bool CanExecuteBearbeitenCommand()
        {
            return selectedAktie!=null;
        }

        private void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenAktieStammdatenBearbeitenMessage>(new OpenAktieStammdatenBearbeitenMessage { AktieID = selectedAktie.ID });
        }

        private void ReceiveAktualisiereViewMessage()
        {
            AktieAPI api = new AktieAPI();
            alleAktien = api.LadeAlle();
            this.RaisePropertyChanged("AlleAktien");
        }
    }
}
