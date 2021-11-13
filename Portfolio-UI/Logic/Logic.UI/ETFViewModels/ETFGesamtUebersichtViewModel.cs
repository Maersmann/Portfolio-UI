using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
using Data.Model.ETFModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.ETFViewModels
{
    public class ETFGesamtUebersichtViewModel : ViewModelUebersicht<ETFModel, StammdatenTypes>
    {

        public ETFGesamtUebersichtViewModel()
        {
            Title = "Übersicht aller ETF's";
            RegisterAktualisereViewMessage(StammdatenTypes.etf.ToString());
            OpenNeueDividendeCommand = new DelegateCommand(ExecuteOpenNeueDividendeCommand, CanExecuteCommand);
        }
        protected override int GetID() { return SelectedItem.ID; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.etf ; }
        protected override string GetREST_API() { return $"/api/etf?aktiv=true"; }
        protected override bool WithPagination() { return true; }


        #region Binding
        public override ETFModel SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                base.SelectedItem = value;
                ((DelegateCommand)OpenNeueDividendeCommand).RaiseCanExecuteChanged();
                if (SelectedItem != null)
                {
                    Messenger.Default.Send(new LoadWertpapierOrderMessage { WertpapierID = SelectedItem.ID, WertpapierTyp = SelectedItem.WertpapierTyp }, messageToken);
                }
            }
        }

        public ICommand OpenNeueDividendeCommand { get; set; }
        #endregion

        #region Commands
        private void ExecuteOpenNeueDividendeCommand()
        {
            Messenger.Default.Send(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = SelectedItem.ID }, messageToken);
        }

        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL+ $"/api/Wertpapier/{SelectedItem.ID}");
                RequestIsWorking = false;
                if ((int)resp.StatusCode == 905)
                {
                    SendExceptionMessage("ETF ist im Depot vorhanden.");
                    return;
                }

            }
            base.ExecuteEntfernenCommand();
            Messenger.Default.Send(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = SelectedItem.WertpapierTyp }, messageToken);
            SendInformationMessage("ETF gelöscht");

        }

        #endregion
    }
}
