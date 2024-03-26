using Aktien.Data.Types;
using Base.Logic.ViewModels;
using Data.Model.SparplanModels;
using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.SparplanMessages;
using System;
using System.Collections.Generic;
using System.Text;
using Logic.Messages.SteuernMessages;

namespace Logic.UI.SparplanViewModels
{
    public class SparplanHistoryUebersichtViewModel : ViewModelUebersicht<SparplanHistoryUebersichtModel, StammdatenTypes>
    {
        private int sparplanID;
        public SparplanHistoryUebersichtViewModel()
        {
            Title = "Übersicht ausgeführter Sparpläne";
            sparplanID = 0;
            WeakReferenceMessenger.Default.Register<LoadSparplanHistoryMessage, string>(this, "SparplanHistory", (r,m) => ReceiveLoadSparplanHistoryMessage(m));
        }

        protected override int GetID() => 0;
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.sparplan;
        protected override string GetREST_API()=> $"/api/SparplanHistory?sparplanID={sparplanID}";
        protected override bool WithPagination() => true;
        protected override bool LoadingOnCreate() => false;

        private async void ReceiveLoadSparplanHistoryMessage(LoadSparplanHistoryMessage m)
        {
            sparplanID = m.SparplanID;
            await LoadData();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            WeakReferenceMessenger.Default.Unregister<LoadSparplanHistoryMessage, string>(this, "SparplanHistory");
        }
    }
}
