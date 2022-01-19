using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.DividendeMessages;
using Base.Logic.Core;
using Base.Logic.Types;
using Base.Logic.ViewModels;
using Data.Model.AuswahlModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Aktien.Logic.UI.AuswahlViewModels
{
    public class DividendenAuswahlViewModel : ViewModelAuswahl<DividendenAuswahlModel, StammdatenTypes>
    {
        private int wertpapierID;

        private bool ohneHinterlegteDividende;

        public DividendenAuswahlViewModel()
        {
            OhneHinterlegteDividende = false;
        }

        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.dividende; }
        protected override string GetREST_API() { return $"/api/Wertpapier/{wertpapierID}/Dividenden?nichtErhalten={ohneHinterlegteDividende}"; }
        protected override bool WithPagination() { return true; }
        public bool OhneHinterlegteDividende { set => ohneHinterlegteDividende = value; }


        public override void LoadData(int wertpapierID)
        {
            this.wertpapierID = wertpapierID;
            base.LoadData(wertpapierID);
        }

        public int? ID()
        {
            return SelectedItem == null ? null : (int?)SelectedItem.ID;
        }
        public DateTime Zahldatum()
        {
            return SelectedItem.Zahldatum;
        }
        public double Betrag()
        {
            return SelectedItem.Betrag;
        }

        #region Commands

        protected override void ExcecuteNewItemCommand()
        {
            Messenger.Default.Send(new OpenDividendeStammdatenMessage<StammdatenTypes> { WertpapierID = wertpapierID, State = State.Neu });
        }

        protected override void ExecuteCloseWindowCommand(Window window)
        {
            AuswahlGetaetigt = true;
            base.ExecuteCloseWindowCommand(window);
        }

        #endregion
    }
}
