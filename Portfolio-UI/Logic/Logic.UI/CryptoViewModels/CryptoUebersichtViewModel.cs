using Aktien.Data.Types.WertpapierTypes;
using Aktien.Data.Types;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Base.Logic.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Data.Model.AktieModels;
using Logic.Messages.UtilMessages;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Data.Model.CryptoModels;
using Base.Logic.Core;
using System.Net.Http;

namespace Logic.UI.CryptoViewModels
{
    public class CryptoUebersichtViewModel : ViewModelUebersicht<CryptoModel, StammdatenTypes>
    {

        public CryptoUebersichtViewModel()
        {
            Title = "Übersicht aller Crypto";
            RegisterAktualisereViewMessage(StammdatenTypes.crypto.ToString());
        }

        protected override int GetID() { return SelectedItem.ID; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.crypto; }
        protected override string GetREST_API() { return $"/api/Wertpapier?aktiv=true&type=3"; }
        protected override bool WithPagination() { return true; }


        #region Binding

        public override CryptoModel SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                base.SelectedItem = value;
                if (SelectedItem != null)
                {
                    WeakReferenceMessenger.Default.Send(new LoadWertpapierOrderMessage { WertpapierID = SelectedItem.ID, WertpapierTyp = WertpapierTypes.Crypto }, messageToken);
                }
            }
        }


        protected override void ExecuteEntfernenCommand()
        {
            WeakReferenceMessenger.Default.Send(new OpenBestaetigungViewMessage
            {
                Beschreibung = "Soll der Eintrag gelöscht werden?",
                Command = async () =>
                {
                    if (GlobalVariables.ServerIsOnline)
                    {
                        RequestIsWorking = true;
                        HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/Wertpapier/{SelectedItem.ID}");
                        RequestIsWorking = false;
                        if ((int)resp.StatusCode == 905)
                        {
                            SendExceptionMessage("Crypto ist im Depot vorhanden.");
                            return;
                        }
                        if ((int)resp.StatusCode == 908)
                        {
                            SendExceptionMessage("Für die Crypto sind Orders ausgeführt.");
                            return;
                        }

                    }
                    WeakReferenceMessenger.Default.Send(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = WertpapierTypes.Crypto }, messageToken);
                    SendInformationMessage("Crypto gelöscht");
                    base.ExecuteEntfernenCommand();
                }
            }, "CryptoUebersicht");


        }

        #endregion
    }
}

