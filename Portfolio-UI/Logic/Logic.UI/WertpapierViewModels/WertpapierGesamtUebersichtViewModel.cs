﻿using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Base.Logic.ViewModels;
using Data.Model.WertpapierModels;
using CommunityToolkit.Mvvm.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.WertpapierViewModels
{
    public class WertpapierGesamtUebersichtViewModel : ViewModelUebersicht<WertpapierModel, StammdatenTypes>
    {

        public WertpapierGesamtUebersichtViewModel()
        {
            Title = "Übersicht aller Wertpapiere";
            RegisterAktualisereViewMessage(StammdatenTypes.aktien.ToString());
            RegisterAktualisereViewMessage(StammdatenTypes.derivate.ToString());
            RegisterAktualisereViewMessage(StammdatenTypes.etf.ToString());
            OpenNeueDividendeCommand = new DelegateCommand(ExecuteOpenNeueDividendeCommand, CanExecuteCommand);
        }

        protected override string GetREST_API() { return $"/api/Wertpapier?aktiv=true"; }
        protected override bool WithPagination() { return true; }


        #region Bindings
        public override WertpapierModel SelectedItem
        { 
            get => base.SelectedItem;
            set 
            {
                base.SelectedItem = value;
                
                ((DelegateCommand)OpenNeueDividendeCommand).RaiseCanExecuteChanged();
                if (SelectedItem != null)
                {
                     WeakReferenceMessenger.Default.Send(new LoadWertpapierOrderMessage { WertpapierID = SelectedItem.ID, WertpapierTyp = SelectedItem.WertpapierTyp }, messageToken);
                }
            }
        }

        public ICommand OpenNeueDividendeCommand { get; set; }
        #endregion

        #region commands
        protected override bool CanExecuteCommand()
        {
            return SelectedItem != null && base.CanExecuteCommand() && (SelectedItem.WertpapierTyp.Equals(WertpapierTypes.Aktie) || (SelectedItem.WertpapierTyp.Equals(WertpapierTypes.ETF)));
        }

        private void ExecuteOpenNeueDividendeCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = SelectedItem.ID }, messageToken);
        }
        #endregion
    }
}
