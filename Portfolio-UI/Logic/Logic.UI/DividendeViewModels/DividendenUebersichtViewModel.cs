using Aktien.Data.Types;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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

        public override void LoadData(int id)
        {
            wertpapierID = id;
            base.LoadData(id);
        }

        #region Commands

        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.DeleteAsync(" GlobalVariables.BackendServer_URL/api/Dividende/" + SelectedItem.ID.ToString());
                RequestIsWorking = false;
                if (resp.IsSuccessStatusCode)
                {
                    SendInformationMessage("Dividende gelöscht");
                    base.ExecuteEntfernenCommand();
                }
            }
        }

        protected override void ExecuteNeuCommand()
        {
            Messenger.Default.Send(new OpenDividendeStammdatenMessage<StammdatenTypes> { WertpapierID = wertpapierID, State = State.Neu });
        }
        protected override void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send(new OpenDividendeStammdatenMessage<StammdatenTypes> { WertpapierID = wertpapierID, State = State.Bearbeiten, DividendeID = SelectedItem.ID });
        }

        #endregion
    }
}
