using Aktien.Data.Types;

using CommunityToolkit.Mvvm.Messaging;
using Aktien.Logic.Messages.DividendeMessages;
using Base.Logic.ViewModels;
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
using Base.Logic.Core;
using Base.Logic.Types;
using Logic.Messages.UtilMessages;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendenUebersichtViewModel : ViewModelUebersicht<DividendeUebersichtModel, StammdatenTypes>
    {

        private int wertpapierID;

        public DividendenUebersichtViewModel()
        {
            Title = "Übersicht aller Dividenden";
            RegisterAktualisereViewMessage(StammdatenTypes.dividende.ToString());
        }

        protected override string GetREST_API() { return $"/api/Wertpapier/{wertpapierID}/Dividenden/"; }
        protected override bool WithPagination() { return true; }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.dividende;

        public override void LoadData(int id)
        {
            wertpapierID = id;
            base.LoadData(id);
        }

        #region Commands

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
                        HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/Dividende/" + SelectedItem.ID.ToString());
                        RequestIsWorking = false;
                        if (resp.IsSuccessStatusCode)
                        {
                            SendInformationMessage("Dividende gelöscht");
                            base.ExecuteEntfernenCommand();
                        }
                        if ((int)resp.StatusCode == 906)
                        {
                            SendExceptionMessage("Dividende wurde schon verteilt.");
                            return;
                        }
                    }
                }
            }, "DividendeUebersicht");
        }

        protected override void ExecuteNeuCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenDividendeStammdatenMessage<StammdatenTypes> { WertpapierID = wertpapierID, State = State.Neu });
        }
        protected override void ExecuteBearbeitenCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenDividendeStammdatenMessage<StammdatenTypes> { WertpapierID = wertpapierID, State = State.Bearbeiten, DividendeID = SelectedItem.ID });
        }

        #endregion
    }
}
