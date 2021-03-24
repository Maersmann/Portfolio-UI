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
using System.Windows.Input;

namespace Aktien.Logic.UI.AuswahlViewModels
{
    public class DividendenAuswahlViewModel : ViewModelAuswahl<Dividende>
    {
        private int wertpapierID;

        private bool ohneHinterlegteDividende;

        public DividendenAuswahlViewModel()
        {
            AuswahlCommand = new DelegateCommand(this.ExecutAuswahlCommand, this.CanExecuteCommand);
            AddCommand = new RelayCommand(this.ExcecuteAddCommand);
            OhneHinterlegteDividende = false;
        }

        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.dividende; }

        public bool OhneHinterlegteDividende { set { ohneHinterlegteDividende = value; } }

        public override void LoadData(int wertpapierID)
        {
            this.wertpapierID = wertpapierID;
            if (ohneHinterlegteDividende)
                itemList = new DividendeAPI().LadeAlleNichtErhaltendeFuerWertpapier(wertpapierID);
            else
                itemList = new DividendeAPI().LadeAlleFuerWertpapier(this.wertpapierID);
            this.RaisePropertyChanged("ItemList");
        }

        #region Commands

        private void ExcecuteAddCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenMessage>(new OpenDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Neu });
        }
       
        private bool CanExecuteCommand()
        {
            return selectedItem != null;
        }

        private void ExecutAuswahlCommand()
        {
            Messenger.Default.Send<DividendeAusgewaehltMessage>(new DividendeAusgewaehltMessage {  ID = selectedItem.ID, Betrag = selectedItem.Betrag, Datum = selectedItem.Zahldatum});
        }

        #endregion

        #region Bindings
        public override Dividende SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                ((DelegateCommand)AuswahlCommand).RaiseCanExecuteChanged();
                base.SelectedItem = value;
            }
        }
        public ICommand AuswahlCommand { get; set; }
        public ICommand AddCommand { get; set; }

        #endregion
    }
}
