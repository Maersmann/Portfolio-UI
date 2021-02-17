using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.WertpapierLogic;
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

namespace Aktien.Logic.UI.WertpapierViewModels
{
    public class WertpapierGesamtUebersichtViewModel : ViewModelUebersicht<Wertpapier>
    {

        public WertpapierGesamtUebersichtViewModel()
        {
            LoadData();
            messageToken = "";
            RegisterAktualisereViewMessage(ViewType.viewWertpapierUebersicht);
            OpenNeueDividendeCommand = new DelegateCommand(this.ExecuteOpenNeueDividendeCommand, this.CanExecuteCommand);
        }


        public string MessageToken { set { messageToken = value; } }
        public override void LoadData()
        {
            itemList = new WertpapierAPI().LadeAlle();
            this.RaisePropertyChanged("ItemList");
        }


        #region Bindings
        public override Wertpapier SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                this.RaisePropertyChanged();
                ((DelegateCommand)OpenNeueDividendeCommand).RaiseCanExecuteChanged();
                if (selectedItem != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedItem.ID }, messageToken);
                }
            }
        }

        public ICommand OpenNeueDividendeCommand { get; set; }
        #endregion

        #region commands
        private bool CanExecuteCommand()
        {
            return (selectedItem != null) && (selectedItem.WertpapierTyp.Equals( WertpapierTypes.Aktie ) );
        }

        private void ExecuteOpenNeueDividendeCommand()
        {
            Messenger.Default.Send<OpenDividendenUebersichtAuswahlMessage>(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = selectedItem.ID }, messageToken);
        }
        #endregion
    }
}
