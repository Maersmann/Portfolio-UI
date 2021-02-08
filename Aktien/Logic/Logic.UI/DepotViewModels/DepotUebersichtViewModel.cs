using Aktien.Data.Model.DepotModels;
using Aktien.Data.Types;
using Aktien.Logic.Core.Depot;
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
    public class DepotUebersichtViewModel: ViewModelBasis
    {
        private readonly ObservableCollection<DepotWertpapier> depotAktien;

        private DepotWertpapier selectedDepotAktie;

        public DepotUebersichtViewModel()
        {
            var api = new DepotAPI();
            depotAktien = api.LadeAlleVorhandeneImDepot();
            OpenDividendeCommand = new DelegateCommand(this.ExecuteOpenDividendeCommandCommand, this.CanExecuteCommand);
        }

        public string MessageToken { set { messageToken = value; } }


        #region Bindings
        public DepotWertpapier SelectedDepotAktie
        {
            get
            {
                return selectedDepotAktie;
            }
            set
            {
                selectedDepotAktie = value;
                ((DelegateCommand)OpenDividendeCommand).RaiseCanExecuteChanged();
                this.RaisePropertyChanged();
                if (selectedDepotAktie != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedDepotAktie.WertpapierID }, messageToken);
                }
            }
        }

        public IEnumerable<DepotWertpapier> DepotAktien
        {
            get
            {
                return depotAktien;
            }
        }

        public ICommand OpenDividendeCommand { get; set; }
        #endregion

        #region Commands
        private bool CanExecuteCommand()
        {
            return (selectedDepotAktie != null) && ( SelectedDepotAktie.Wertpapier.WertpapierTyp.Equals(WertpapierTypes.Aktie));
        }

        private void ExecuteOpenDividendeCommandCommand()
        {
            Messenger.Default.Send<OpenDividendenUebersichtAuswahlMessage>(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = selectedDepotAktie.WertpapierID }, "DepotUebersicht");
        }
        #endregion
    }
}
