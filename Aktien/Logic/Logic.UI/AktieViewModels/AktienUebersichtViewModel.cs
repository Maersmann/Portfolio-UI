using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DepotMessages;
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
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;
using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;

namespace Aktien.Logic.UI.AktieViewModels
{
    public class AktienUebersichtViewModel : ViewModelUebersicht<Wertpapier>
    {
      
        public AktienUebersichtViewModel()
        {
            Title = "Übersicht aller Aktien";
            LoadData();
            OpenNeueDividendeCommand = new DelegateCommand(this.ExecuteOpenNeueDividendeCommand, this.CanExecuteCommand);
            RegisterAktualisereViewMessage(ViewType.viewAktienUebersicht);
        }

        protected override int getID() { return selectedItem.ID; }
        protected override ViewType getVStammdatenViewType() { return ViewType.viewAktieStammdaten; }

        public override void LoadData()
        {
            itemList = new AktieAPI().LadeAlle();
            this.RaisePropertyChanged("ItemList");
        }

        #region Binding

        public override Wertpapier SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                base.SelectedItem = value;
                ((DelegateCommand)OpenNeueDividendeCommand).RaiseCanExecuteChanged();
                if (selectedItem != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedItem.ID, WertpapierTyp = WertpapierTypes.Aktie }, messageToken);
                }
            }
        }

        public ICommand OpenNeueDividendeCommand { get; set; }
        #endregion

        #region Commands

        private void ExecuteOpenNeueDividendeCommand()
        {
            Messenger.Default.Send<OpenDividendenUebersichtAuswahlMessage>(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = selectedItem.ID }, messageToken);
        }
        protected override void ExecuteEntfernenCommand()
        {
            try
            {
                new AktieAPI().Entfernen(selectedItem);

            }
            catch (WertpapierInDepotVorhandenException)
            {
                SendExceptionMessage("Aktie ist im Depot vorhanden.");
                return;
            }
            Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = 0, WertpapierTyp = WertpapierTypes.Aktie }, messageToken);
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewWertpapierUebersicht);
            SendInformationMessage("Aktie gelöscht");
            base.ExecuteEntfernenCommand();
        }

        #endregion
    }
}
