using Aktien.Data.Types;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.SparplanModels;
using Data.Model.ZinsenModels;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.SparplanMessages;
using Logic.Messages.UtilMessages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Logic.UI.ZinsenViewModels
{
    public class ZinsenErhaltenUebersichtViewModel : ViewModelUebersicht<ZinsenErhaltenUebersichtModel, StammdatenTypes>
    {
        public ZinsenErhaltenUebersichtViewModel()
        {
            Title = "Übersicht erhaltene Zinsen";
        }

        protected override int GetID() => SelectedItem.ID;
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.zinsenErhalten;
        protected override string GetREST_API() => $"/api/Zinsen";
        protected override bool WithPagination() => true;

        protected override void ExecuteEntfernenCommand()
        {
            Messenger.Default.Send(new OpenBestaetigungViewMessage
            {
                Beschreibung = "Soll der Eintrag gelöscht werden?",
                Command = async () =>
                {
                    if (GlobalVariables.ServerIsOnline)
                    {
                        RequestIsWorking = true;
                        HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/Zinsen/{SelectedItem.ID}");
                        RequestIsWorking = false;
                    }  
                    SendInformationMessage("Zinsen gelöscht");
                    base.ExecuteEntfernenCommand();
                }
            }, "ZinsenErhaltenUebersicht");
        }

        protected override async void ExecuteNeuCommand()
        {
            base.ExecuteNeuCommand();
            await LoadData();
        }
    }
}
