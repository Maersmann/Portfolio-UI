using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.DividendeModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeErhaltenUebersichtViewModel : ViewModelUebersicht<DividendeErhaltenUebersichtModel>
    {

        private int wertpapierID;

        public DividendeErhaltenUebersichtViewModel()
        {
            Title = "Übersicht aller erhaltene Dividenden";
            RegisterAktualisereViewMessage(StammdatenTypes.steuergruppe);
        }

        public async override void LoadData(int id)
        {
            this.wertpapierID = id;

            HttpResponseMessage resp = await Client.GetAsync($"https://localhost:5001/api/Wertpapier/{wertpapierID}/ErhalteneDividenden/");
            if (resp.IsSuccessStatusCode)
            {
                itemList = await resp.Content.ReadAsAsync<ObservableCollection<DividendeErhaltenUebersichtModel>>();
            }
            this.RaisePropertyChanged("ItemList");
        }
        public override void LoadData()
        {
            LoadData(wertpapierID);
        }

        #region Commands
        protected override void ExecuteNeuCommand()
        {
            Messenger.Default.Send<OpenErhaltendeDividendeStammdatenMessage>(new OpenErhaltendeDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Neu });
        }
        protected override void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenErhaltendeDividendeStammdatenMessage>(new OpenErhaltendeDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Bearbeiten, ID = selectedItem.ID });
        }

        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.DeleteAsync($"https://localhost:5001/api/DividendeErhalten/{selectedItem.ID}");
                if (resp.IsSuccessStatusCode)
                {
                    SendInformationMessage("Dividende Erhalten gelöscht");
                    base.ExecuteEntfernenCommand();
                }
            }
        }
        #endregion
    }
}
