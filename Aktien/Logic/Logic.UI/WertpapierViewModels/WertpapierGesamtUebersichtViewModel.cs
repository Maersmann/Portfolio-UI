using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
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
    public class WertpapierGesamtUebersichtViewModel : ViewModelUebersicht
    {
        private ObservableCollection<Wertpapier> wertpapiere;

        private Wertpapier selectedWertpapier;

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
            wertpapiere = new WertpapierAPI().LadeAlle();
            this.RaisePropertyChanged("Wertpapiere");
        }


        #region Bindings
        public Wertpapier SelectedWertpapier
        {
            get
            {
                return selectedWertpapier;
            }
            set
            {
                selectedWertpapier = value;
                this.RaisePropertyChanged();
                ((DelegateCommand)OpenNeueDividendeCommand).RaiseCanExecuteChanged();
                if (selectedWertpapier != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedWertpapier.ID }, messageToken);
                }
            }
        }
        public IEnumerable<Wertpapier> Wertpapiere
        {
            get
            {
                return wertpapiere;
            }
        }

        public ICommand OpenNeueDividendeCommand { get; set; }
        #endregion

        #region commands
        private bool CanExecuteCommand()
        {
            return (selectedWertpapier != null) && ( selectedWertpapier.WertpapierTyp.Equals( WertpapierTypes.Aktie ) );
        }

        private void ExecuteOpenNeueDividendeCommand()
        {
            Messenger.Default.Send<OpenDividendenUebersichtAuswahlMessage>(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = selectedWertpapier.ID }, messageToken);
        }
        #endregion
    }
}
