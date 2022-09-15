using Aktien.Data.Types;
using Base.Logic.ViewModels;
using Data.Model.SparplanModels;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.SparplanMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.UI.SparplanViewModels
{
    public class SparplanHistoryUebersichtViewModel : ViewModelUebersicht<SparplanHistoryUebersichtModel, StammdatenTypes>
    {
        private int sparplanID;
        public SparplanHistoryUebersichtViewModel()
        {
            Title = "Übersicht ausgeführter Sparpläne";
            sparplanID = 0;
            Messenger.Default.Register<LoadSparplanHistoryMessage>(this, "SparplanHistory", m => ReceiveLoadSparplanHistoryMessage(m));
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
    }
}
