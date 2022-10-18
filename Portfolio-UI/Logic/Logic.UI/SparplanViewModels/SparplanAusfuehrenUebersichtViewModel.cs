using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.DTO.SparplanDTOs;
using Data.Model.SparplanModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.SparplanMessages;
using Logic.Messages.UtilMessages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.SparplanViewModels
{
    public class SparplanAusfuehrenUebersichtViewModel : ViewModelUebersicht<SparplanAusfuehrenUebersichtModel, StammdatenTypes>
    {

        public SparplanAusfuehrenUebersichtViewModel()
        {
            Title = "Übersicht der auszuführenden Sparpläne";
            FuehreSparplanAusCommmand = new RelayCommand(() => ExecuteFuehreSparplanAusCommmand());
            SparplanFehlgeschlagenCommand = new RelayCommand(() => ExecuteSparplanFehlgeschlagenCommand());
        }

        protected override string GetREST_API() { return $"/api/sparplan/ausfuehren"; }
        protected override bool WithPagination() { return true; }
        protected override int GetID() { return SelectedItem.ID; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.sparplan; }

        #region Bindings
        public override SparplanAusfuehrenUebersichtModel SelectedItem
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

        public ICommand FuehreSparplanAusCommmand { get; set; }
        public ICommand SparplanFehlgeschlagenCommand { get; set; }
        #endregion

        #region Commands

        private void ExecuteFuehreSparplanAusCommmand()
        {
            Messenger.Default.Send(new OpenSparplanAusfuehrenMessage { SparplanAusfuehren = SelectedItem }, "SparplanAusfuehrenUebersicht");
        }

        private async void ExecuteSparplanFehlgeschlagenCommand()
        {

            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + $"/api/sparplan/fehlgeschlagen",
                    new SparplanFehlgeschlagenDTO
                    {
                        SparplanID = SelectedItem.ID
                    });
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    SendInformationMessage("Gespeichert");
                    await LoadData();
                }
                else
                {
                    SendExceptionMessage("Sparplan konnte nicht als fehlgeschlagen eingetragen werden.");
                    return;
                }
            }
        }


        #endregion
    }
}
