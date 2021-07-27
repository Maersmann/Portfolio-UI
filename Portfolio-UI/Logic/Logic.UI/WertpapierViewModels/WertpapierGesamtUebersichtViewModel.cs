using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.WertpapierModels;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.WertpapierViewModels
{
    public class WertpapierGesamtUebersichtViewModel : ViewModelUebersicht<WertpapierModel>
    {

        public WertpapierGesamtUebersichtViewModel()
        {
            Title = "Übersicht aller Wertpapiere";
            LoadData();
            RegisterAktualisereViewMessage(StammdatenTypes.aktien);
            RegisterAktualisereViewMessage(StammdatenTypes.derivate);
            RegisterAktualisereViewMessage(StammdatenTypes.etf);
            OpenNeueDividendeCommand = new DelegateCommand(this.ExecuteOpenNeueDividendeCommand, this.CanExecuteCommand);
        }

        public override async void LoadData()
        {
            if ( GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+"/api/Wertpapier?aktiv=true");
                if (resp.IsSuccessStatusCode)
                {
                    itemList = await resp.Content.ReadAsAsync<ObservableCollection<WertpapierModel>>();
                }
            }

            base.LoadData();
        }

        protected override bool OnFilterTriggered(object item)
        {
            if (item is WertpapierModel wertpapier)
            {
                return wertpapier.Name.ToLower().Contains(filtertext.ToLower());
            }
            return true;
        }


        #region Bindings
        public override WertpapierModel SelectedItem
        { 
            get => base.SelectedItem;
            set 
            {
                base.SelectedItem = value;
                
                ((DelegateCommand)OpenNeueDividendeCommand).RaiseCanExecuteChanged();
                if (selectedItem != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedItem.ID, WertpapierTyp = selectedItem.WertpapierTyp }, messageToken);
                }
            }
        }

        public string FilterText
        {
            get => filtertext;
            set
            {
                filtertext = value;
                RaisePropertyChanged();
                _customerView.Refresh();
            }
        }


        public ICommand OpenNeueDividendeCommand { get; set; }
        #endregion

        #region commands
        protected override bool CanExecuteCommand()
        {
            return base.CanExecuteCommand() && ((selectedItem.WertpapierTyp.Equals( WertpapierTypes.Aktie )||(selectedItem.WertpapierTyp.Equals(WertpapierTypes.ETF))));
        }

        private void ExecuteOpenNeueDividendeCommand()
        {
            Messenger.Default.Send<OpenDividendenUebersichtAuswahlMessage>(new OpenDividendenUebersichtAuswahlMessage { WertpapierID = selectedItem.ID }, messageToken);
        }
        #endregion
    }
}
