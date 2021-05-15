using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DepotMessages;
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
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using System.Net.Http;
using Aktien.Logic.Core;
using System.Net;
using Data.Model.AktieModels;

namespace Aktien.Logic.UI.AktieViewModels
{
    public class AktienUebersichtViewModel : ViewModelUebersicht<AktienModel>
    {
      
        public AktienUebersichtViewModel()
        {
            Title = "Übersicht aller Aktien";
            LoadData();
            OpenNeueDividendeCommand = new DelegateCommand(this.ExecuteOpenNeueDividendeCommand, this.CanExecuteCommand);
            RegisterAktualisereViewMessage(StammdatenTypes.aktien);
        }

        protected override int GetID() { return selectedItem.ID; }
        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.aktien; }

        public async override void LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync("https://localhost:5001/api/Wertpapier?aktiv=true&type=0");
                if (resp.IsSuccessStatusCode)
                    itemList = await resp.Content.ReadAsAsync<ObservableCollection<AktienModel>>();
            }
            this.RaisePropertyChanged("ItemList");
        }

        #region Binding

        public override AktienModel SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                base.SelectedItem = value;
                ((DelegateCommand)OpenNeueDividendeCommand).RaiseCanExecuteChanged();
                if (selectedItem != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedItem.ID, WertpapierTyp = WertpapierTypes.Aktie }, messageToken);
                }
            }
        }

        public ICommand OpenNeueDividendeCommand { get; set; }
        #endregion

        #region Commands

        private void ExecuteOpenNeueDividendeCommand()
        {
            Messenger.Default.Send<OpenDividendenUebersichtAuswahlMessage>(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = selectedItem.ID }, messageToken);
        }
        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.DeleteAsync($"https://localhost:5001/api/Wertpapier/{selectedItem.ID}");
                if (resp.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    SendExceptionMessage("Aktie ist im Depot vorhanden.");
                    return;
                }
      
            }
            Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = WertpapierTypes.Aktie }, messageToken);
            SendInformationMessage("Aktie gelöscht");
            base.ExecuteEntfernenCommand();
        }

        #endregion
    }
}
