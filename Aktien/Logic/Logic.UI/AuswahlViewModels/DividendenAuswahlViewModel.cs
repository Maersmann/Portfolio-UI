using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Aktien.Logic.UI.AuswahlViewModels
{
    public class DividendenAuswahlViewModel : ViewModelAuswahl<Dividende>
    {
        private int wertpapierID;

        private bool ohneHinterlegteDividende;
        private Action<bool, int, Double, DateTime> Callback;

        public DividendenAuswahlViewModel()
        {
            OhneHinterlegteDividende = false;
        } 

        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.dividende; }
        public bool OhneHinterlegteDividende { set { ohneHinterlegteDividende = value; } }
        public void SetCallback(Action<bool, int, Double, DateTime> callback)
        {
            Callback = callback;
        }

        public override void LoadData(int wertpapierID)
        {
            this.wertpapierID = wertpapierID;
            if (ohneHinterlegteDividende)
                itemList = new DividendeAPI().LadeAlleNichtErhaltendeFuerWertpapier(wertpapierID);
            else
                itemList = new DividendeAPI().LadeAlleFuerWertpapier(wertpapierID);
            this.RaisePropertyChanged("ItemList");
        }

        #region Commands

        protected override void ExcecuteNewItemCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenMessage>(new OpenDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Neu });
        }

        protected override void ExecuteCloseWindowCommand(Window window)
        {
            base.ExecuteCloseWindowCommand(window);

            if (selectedItem != null)
                Callback(true, selectedItem.ID, selectedItem.Betrag, selectedItem.Zahldatum);
            else
                Callback(false, 0, 0, DateTime.MinValue);      
        }

        #endregion
    }
}
