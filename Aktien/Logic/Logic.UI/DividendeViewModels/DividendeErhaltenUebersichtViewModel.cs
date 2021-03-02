using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeErhaltenUebersichtViewModel : ViewModelUebersicht<DividendeErhalten>
    {

        private int wertpapierID;

        public DividendeErhaltenUebersichtViewModel()
        {
            Title = "Übersicht aller erhaltene Dividenden";
        }

        public override void LoadData(int id)
        {
            this.wertpapierID = id;
            itemList = new AktieAPI().LadeAlleErhalteneDividenden(this.wertpapierID);
            this.RaisePropertyChanged("ItemList");
        }

        #region Commands
        protected override void ExecuteNeuCommand()
        {
            Messenger.Default.Send<OpenErhaltendeDividendeStammdatenMessage>(new OpenErhaltendeDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Neu });
        }
        protected override void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenErhaltendeDividendeStammdatenMessage>(new OpenErhaltendeDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Bearbeiten, ID = selectedItem.ID });
        }

        protected override void ExecuteEntfernenCommand()
        {
            new DividendeErhaltenAPI().Entfernen(selectedItem.ID);
            SendInformationMessage("Dividende gelöscht");
            base.ExecuteEntfernenCommand();
        }
        #endregion
    }
}
