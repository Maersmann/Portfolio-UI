using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DepotMessages;
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
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using System.Net.Http;
using Aktien.Logic.Core;
using System.Net;
using Data.Model.AktieModels;
using Base.Logic.Core;
using Logic.Messages.UtilMessages;

namespace Aktien.Logic.UI.AktieViewModels
{
    public class AktienUebersichtViewModel : ViewModelUebersicht<AktienModel, StammdatenTypes>
    {
      
        public AktienUebersichtViewModel()
        {
            Title = "Übersicht aller Aktien";
            OpenNeueDividendeCommand = new DelegateCommand(ExecuteOpenNeueDividendeCommand, CanExecuteCommand);
            RegisterAktualisereViewMessage(StammdatenTypes.aktien.ToString());
        }

        protected override int GetID() { return SelectedItem.ID; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.aktien; }
        protected override string GetREST_API() { return $"/api/Wertpapier?aktiv=true&type=0"; }
        protected override bool WithPagination() { return true; }


        #region Binding

        public override AktienModel SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                base.SelectedItem = value;
                ((DelegateCommand)OpenNeueDividendeCommand).RaiseCanExecuteChanged();
                if (SelectedItem != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = SelectedItem.ID, WertpapierTyp = WertpapierTypes.Aktie }, messageToken);
                }
            }
        }

        public ICommand OpenNeueDividendeCommand { get; set; }
        #endregion

        #region Commands

        private void ExecuteOpenNeueDividendeCommand()
        {
            Messenger.Default.Send(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = SelectedItem.ID }, messageToken);
        }
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
                        HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/Wertpapier/{SelectedItem.ID}");
                        RequestIsWorking = false;
                        if ((int)resp.StatusCode == 905)
                        {
                            SendExceptionMessage("Aktie ist im Depot vorhanden.");
                            return;
                        }
                        if ((int)resp.StatusCode == 907)
                        {
                            SendExceptionMessage("Aktie hat Dividenden verteilt.");
                            return;
                        }
                        if ((int)resp.StatusCode == 908)
                        {
                            SendExceptionMessage("Für die Aktie sind Orders ausgeführt.");
                            return;
                        }

                    }
                    Messenger.Default.Send(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = WertpapierTypes.Aktie }, messageToken);
                    SendInformationMessage("Aktie gelöscht");
                    base.ExecuteEntfernenCommand();
                }
            }, "AktienUebersicht");

           
        }

        #endregion
    }
}
