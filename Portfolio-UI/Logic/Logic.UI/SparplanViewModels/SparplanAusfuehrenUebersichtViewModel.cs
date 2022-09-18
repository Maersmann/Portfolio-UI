using Aktien.Data.Types;
using Base.Logic.ViewModels;
using Data.Model.SparplanModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.SparplanMessages;
using Logic.Messages.UtilMessages;
using System;
using System.Collections.Generic;
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
        #endregion

        #region Commands

        private void ExecuteFuehreSparplanAusCommmand()
        {
            Messenger.Default.Send(new OpenSparplanAusfuehrenMessage { SparplanAusfuehren = SelectedItem }, "SparplanAusfuehrenUebersicht");
        }

       
        #endregion
    }
}
