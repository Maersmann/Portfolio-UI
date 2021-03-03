using Aktien.Data.Types;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.BaseViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Aktien.Data.Model.WertpapierEntitys;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendenUebersichtViewModel : ViewModelUebersicht<Dividende>
    {

        private int wertpapierID;

        public DividendenUebersichtViewModel()
        {
            Title = "Übersicht aller Dividenden";
            RegisterAktualisereViewMessage(StammdatenTypes.dividende);
        }

        public override void LoadData(int id)
        {
            wertpapierID = id;
            itemList = new DividendeAPI().LadeAlleFuerWertpapier(wertpapierID);
            this.RaisePropertyChanged("ItemList");
        }

        #region Commands

        protected override void ExecuteEntfernenCommand()
        {
            new DividendeAPI().Entfernen(selectedItem.ID);
            SendInformationMessage("Dividende gelöscht");
            base.ExecuteEntfernenCommand();
        }

        protected override void ExecuteNeuCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenMessage>(new OpenDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Neu });
        }
        protected override void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenDividendeStammdatenMessage>(new OpenDividendeStammdatenMessage { WertpapierID = wertpapierID, State = State.Bearbeiten, DividendeID = selectedItem.ID });
        }

        #endregion
    }
}
