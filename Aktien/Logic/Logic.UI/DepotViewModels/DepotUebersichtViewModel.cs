using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Model.DepotModels;
using Aktien.Data.Types;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class DepotUebersichtViewModel: ViewModelUebersicht<DepotGesamtUebersichtItem>
    {

        public DepotUebersichtViewModel()
        {
            LoadData();
            OpenDividendeCommand = new DelegateCommand(this.ExecuteOpenDividendeCommandCommand, this.CanExecuteCommand);
            RegisterAktualisereViewMessage(ViewType.viewDepotUebersicht);
        }


        public string MessageToken { set { messageToken = value; } }

        public override void LoadData()
        {
            var api = new DepotAPI();
            itemList = api.LadeFuerGesamtUebersicht();
            this.RaisePropertyChanged("ItemList");
        }


        #region Bindings
        public override DepotGesamtUebersichtItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                ((DelegateCommand)OpenDividendeCommand).RaiseCanExecuteChanged();
                this.RaisePropertyChanged();
                if (selectedItem != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedItem.WertpapierID }, messageToken);
                }
            }
        }


        public ICommand OpenDividendeCommand { get; set; }
        #endregion

        #region Commands
        private bool CanExecuteCommand()
        {
            return (selectedItem != null) && (selectedItem.WertpapierTyp.Equals(WertpapierTypes.Aktie));
        }

        private void ExecuteOpenDividendeCommandCommand()
        {
            Messenger.Default.Send<OpenDividendenUebersichtAuswahlMessage>(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = selectedItem.WertpapierID }, "DepotUebersicht");
        }
        #endregion
    }
}
