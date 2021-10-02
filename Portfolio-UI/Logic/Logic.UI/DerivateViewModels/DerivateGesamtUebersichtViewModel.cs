using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.WertpapierMessages;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.DerivateModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DerivateViewModels
{
    public class DerivateGesamtUebersichtViewModel : ViewModelUebersicht<DerivateModel, StammdatenTypes>
    {

        public DerivateGesamtUebersichtViewModel()
        {
            Title = "Übersicht aller Derivate";
            LoadData();
            RegisterAktualisereViewMessage(StammdatenTypes.derivate.ToString());
        }

        protected override int GetID() { return selectedItem.ID; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.derivate; }
        protected override string GetREST_API() { return $"/api/Wertpapier?aktiv=true&type=2"; }

        protected override bool OnFilterTriggered(object item)
        {
            if (item is DerivateModel wertpapier)
            {
                return wertpapier.Name.ToLower().Contains(filtertext.ToLower());
            }
            return true;
        }

        #region Binding
        public override DerivateModel SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                base.SelectedItem = value;
                if (selectedItem != null)
                {
                    Messenger.Default.Send(new LoadWertpapierOrderMessage { WertpapierID = selectedItem.ID, WertpapierTyp = selectedItem.WertpapierTyp }, messageToken);
                }
            }
        }

        #endregion

        #region Commands


        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL+ $"/api/Wertpapier/{selectedItem.ID}");
                RequestIsWorking = false;
                if ((int)resp.StatusCode == 905)
                {
                    SendExceptionMessage("Derivate ist im Depot vorhanden.");
                    return;
                }

            }
            Messenger.Default.Send(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = selectedItem.WertpapierTyp }, messageToken);
            SendInformationMessage("Derivate gelöscht");
            base.ExecuteEntfernenCommand();
        }

        #endregion
    }
}
