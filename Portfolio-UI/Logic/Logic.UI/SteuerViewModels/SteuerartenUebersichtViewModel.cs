using Aktien.Data.Types;
using Aktien.Logic.Core;
using Base.Logic.Core;
using Base.Logic.ViewModels;
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
    public class SteuerartenUebersichtViewModel : ViewModelUebersicht<SteuerartModel, StammdatenTypes>
    {
        public SteuerartenUebersichtViewModel()
        {
            Title = "Übersicht aller Steuerarten";
            RegisterAktualisereViewMessage(StammdatenTypes.steuerart.ToString());
        }


        protected override int GetID() { return SelectedItem.ID; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.steuerart; }
        protected override string GetREST_API() { return $"/api/Steuerarten"; }
        protected override bool WithPagination() { return true; }


        #region Commands

        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/Steuerarten/{SelectedItem.ID}");
                RequestIsWorking = false;
                if (!resp.IsSuccessStatusCode)
                {
                    if ((int)resp.StatusCode == 903)
                    {
                        SendExceptionMessage("Steuerart in Steuern verwendet.");
                    }
                    else
                    {
                        SendExceptionMessage("Steuerart konnte nicht gelöscht werden.");
                    }

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
