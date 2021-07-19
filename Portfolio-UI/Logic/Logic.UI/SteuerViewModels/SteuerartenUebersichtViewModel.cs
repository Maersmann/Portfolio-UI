using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.SteuerModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.SteuerViewModels
{
    public class SteuerartenUebersichtViewModel : ViewModelUebersicht<SteuerartModel>
    {
        public SteuerartenUebersichtViewModel()
        {
            Title = "Übersicht aller Steuerarten";
            LoadData();
            RegisterAktualisereViewMessage(StammdatenTypes.steuerart);
        }


        protected override int GetID() { return selectedItem.ID; }
        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.steuerart; }

        public async override void LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/Steuerarten");
                if (resp.IsSuccessStatusCode)
                    itemList = await resp.Content.ReadAsAsync<ObservableCollection<SteuerartModel>>();
                else
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
            }
            this.RaisePropertyChanged("ItemList");
        }

        #region Commands

        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/Steuerarten/{selectedItem.ID}");
                if (!resp.IsSuccessStatusCode)
                {
                    if ((int)resp.StatusCode == 903)
                        SendExceptionMessage("Steuerart in Steuern verwendet.");
                    else
                        SendExceptionMessage("Steuerart konnte nicht gelöscht werden.");
                    return;
                }
                else
                {
                    SendInformationMessage("Steuerart gelöscht");
                    base.ExecuteEntfernenCommand();
                }
            }           
        }

        #endregion
    }
}
