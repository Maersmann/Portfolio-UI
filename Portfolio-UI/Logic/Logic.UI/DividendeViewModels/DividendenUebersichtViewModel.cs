using Aktien.Data.Types;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
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
using System.Net.Http;
using Data.Model.DividendeModels;
using Aktien.Logic.Core;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendenUebersichtViewModel : ViewModelUebersicht<DividendeUebersichtModel>
    {

        private int wertpapierID;

        public DividendenUebersichtViewModel()
        {
            Title = "Übersicht aller Dividenden";
            RegisterAktualisereViewMessage(StammdatenTypes.dividende);
        }

        public async override void LoadData(int id)
        {
            wertpapierID = id;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/Wertpapier/{wertpapierID}/Dividenden/");
            if (resp.IsSuccessStatusCode)
            {
                itemList = await resp.Content.ReadAsAsync<ObservableCollection<DividendeUebersichtModel>>();
            }
            this.RaisePropertyChanged("ItemList");
        }
        public override void LoadData()
        {
            LoadData(wertpapierID);
        }

        #region Commands

        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.DeleteAsync(" GlobalVariables.BackendServer_URL/api/Dividende/" + selectedItem.ID.ToString());
                if (resp.IsSuccessStatusCode)
                {
                    SendInformationMessage("Dividende gelöscht");
                    base.ExecuteEntfernenCommand();
                }
            }
        }

        protected override void ExecuteNeuCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenMessage>(new OpenDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Neu });
        }
        protected override void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenMessage>(new OpenDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Bearbeiten, DividendeID = selectedItem.ID });
        }

        #endregion
    }
}
