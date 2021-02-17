﻿using Aktien.Data.Model.WertpapierEntitys;
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
    public class ETFGesamtUebersichtViewModel : ViewModelBasis
    {
        private ObservableCollection<Wertpapier> alleETF;

        private Wertpapier selectedETF;

        public ETFGesamtUebersichtViewModel()
        {
            LoadData();
            messageToken = "";
            BearbeitenCommand = new DelegateCommand(this.ExecuteBearbeitenCommand, this.CanExecuteCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanExecuteCommand);
            AddAktieCommand = new RelayCommand(this.ExecuteAddAktieCommand);
        }

        public string MessageToken { set { messageToken = value; } }

        public void LoadData()
        {
            alleETF = new EtfAPI().LadeAlle();
            this.RaisePropertyChanged("AlleETF");
        }

        #region Binding
        public Wertpapier SelectedETF
        {
            get
            {
                return selectedETF;
            }
            set
            {
                selectedETF = value;
                this.RaisePropertyChanged();
                ((DelegateCommand)BearbeitenCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)EntfernenCommand).RaiseCanExecuteChanged();
                if (selectedETF != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedETF.ID, WertpapierTyp = selectedETF.WertpapierTyp }, messageToken);
                }
            }
        }
        public IEnumerable<Wertpapier> AlleETF
        {
            get
            {
                return alleETF;
            }
        }
        public ICommand BearbeitenCommand { get; protected set; }
        public ICommand EntfernenCommand { get; protected set; }
        public ICommand AddAktieCommand { get; set; }
        #endregion

        #region Commands
        private bool CanExecuteCommand()
        {
            return selectedETF != null;
        }

        private void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenETFStammdatenMessage>(new OpenETFStammdatenMessage { WertpapierID = selectedETF.ID, State = Data.Types.State.Bearbeiten });
        }


        private void ExecuteEntfernenCommand()
        {
            try
            {
                new EtfAPI().Entfernen(selectedETF);
            }
            catch (WertpapierInDepotVorhandenException)
            {
                SendExceptionMessage("ETF ist im Depot vorhanden.");
                return;
            }

            Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = selectedETF.WertpapierTyp }, messageToken);
            alleETF.Remove(SelectedETF);
            this.RaisePropertyChanged("AlleAktien");
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
