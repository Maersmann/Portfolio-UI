﻿using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.DividendeMessages;
using Base.Logic.Core;
using Base.Logic.Types;
using Base.Logic.ViewModels;
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
    public class DividendeErhaltenUebersichtViewModel : ViewModelUebersicht<DividendeErhaltenUebersichtModel, StammdatenTypes>
    {

        private int wertpapierID;

        public DividendeErhaltenUebersichtViewModel()
        {
            Title = "Übersicht aller erhaltene Dividenden";
            RegisterAktualisereViewMessage(StammdatenTypes.steuergruppe.ToString());
        }

        public async override void LoadData(int id)
        {
            wertpapierID = id;

            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/Wertpapier/{wertpapierID}/ErhalteneDividenden/");
            if (resp.IsSuccessStatusCode)
            {
                itemList = await resp.Content.ReadAsAsync<ObservableCollection<DividendeErhaltenUebersichtModel>>();
            }
            base.LoadData(id);
        }
        public override void LoadData()
        {
            LoadData(wertpapierID);
        }

        #region Commands
        protected override void ExecuteNeuCommand()
        {
            Messenger.Default.Send(new OpenErhaltendeDividendeStammdatenMessage<StammdatenTypes> { WertpapierID = wertpapierID, State = State.Neu });
        }
        protected override void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send(new OpenErhaltendeDividendeStammdatenMessage<StammdatenTypes> { WertpapierID = wertpapierID, State = State.Bearbeiten, ID = selectedItem.ID });
        }

        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL+ $"/api/DividendeErhalten/{selectedItem.ID}");
                RequestIsWorking = false;
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
