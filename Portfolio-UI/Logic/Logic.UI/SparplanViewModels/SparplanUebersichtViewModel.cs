using Aktien.Data.Types;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.DTO.SparplanDTOs;
using Data.Model.SparplanModels;
using Data.Types.SparplanTypes;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.SparplanMessages;
using Logic.Messages.UtilMessages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.SparplanViewModels
{
    public class SparplanUebersichtViewModel : ViewModelUebersicht<SparplanUebersichtModel, StammdatenTypes>
    {

        public SparplanUebersichtViewModel()
        {
            Title = "Übersicht der Sparpläne";
            BeendeSparplanCommand = new RelayCommand(() => ExecuteBeendeSparplanCommand());
        }

        protected override string GetREST_API() { return $"/api/sparplan"; }
        protected override bool WithPagination() { return true; }
        protected override int GetID() { return SelectedItem.ID; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.sparplan; }

        #region Bindings
        public override SparplanUebersichtModel SelectedItem
        {
            get
            {
                return base.SelectedItem;
            }
            set
            {
                base.SelectedItem = value;
                RaisePropertyChanged();
                if (SelectedItem != null)
                {
                    Messenger.Default.Send(new LoadSparplanHistoryMessage { SparplanID = SelectedItem.ID }, "SparplanHistory");
                }
            }
        }

        public ICommand BeendeSparplanCommand { get; set; }
        #endregion

        #region Commands

        private void ExecuteBeendeSparplanCommand()
        {
            Messenger.Default.Send(new OpenBestaetigungViewMessage
            {
                Beschreibung = "Soll der Sparplan beendet werden?",
                Command = async () =>
                {
                    if (GlobalVariables.ServerIsOnline)
                    {
                        RequestIsWorking = true;
                        HttpResponseMessage resp = await Client.PutAsJsonAsync(GlobalVariables.BackendServer_URL + $"/api/sparplan/beenden", new SparplanBeendenDTO { ID = SelectedItem.ID } );
                        RequestIsWorking = false;
                        if (!resp.IsSuccessStatusCode)
                        {
                            SendExceptionMessage("Sparplan konnte nicht beendet werden");
                        }
                        else
                        {
                            SendInformationMessage("Sparplan beendet");
                            await LoadData();
                        }

                    }
                    Messenger.Default.Send(new LoadSparplanHistoryMessage { SparplanID = 0 }, "SparplanHistory");

                }
            }, "SparplanUebersicht");
        }

        protected override void ExecuteEntfernenCommand()
        {
            Messenger.Default.Send(new OpenBestaetigungViewMessage
            {
                Beschreibung = "Soll der Sparplan gelöscht werden?",
                Command = async () =>
                {
                    if (GlobalVariables.ServerIsOnline)
                    {
                        RequestIsWorking = true;
                        HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/sparplan/{SelectedItem.ID}");
                        RequestIsWorking = false;
                        if (resp.StatusCode.Equals(HttpStatusCode.Conflict))
                        {
                            SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                        }
                        else if(!resp.IsSuccessStatusCode)
                        {
                            SendExceptionMessage("Sparplan konnte nicht gelöscht werden");
                        }
                        else
                        {
                            SendInformationMessage("Sparplan gelöscht");
                            base.ExecuteEntfernenCommand();
                        }

                    }
                    Messenger.Default.Send(new LoadSparplanHistoryMessage { SparplanID = 0 }, "SparplanHistory");

                }
            }, "SparplanUebersicht");
        }

        protected override async void ExecuteNeuCommand()
        {
            base.ExecuteNeuCommand();
            await LoadData();
        }

        protected override async void ExecuteBearbeitenCommand()
        {
            base.ExecuteBearbeitenCommand();
            await LoadData();
        }
        #endregion
    }
}
