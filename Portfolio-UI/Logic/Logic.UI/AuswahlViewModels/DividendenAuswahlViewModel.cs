using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.AuswahlModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Aktien.Logic.UI.AuswahlViewModels
{
    public class DividendenAuswahlViewModel : ViewModelAuswahl<DividendenAuswahlModel>
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

        public async override void LoadData(int wertpapierID)
        {
            this.wertpapierID = wertpapierID;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+$"/api/Wertpapier/{wertpapierID}/Dividenden?nichtErhalten={ohneHinterlegteDividende}");
                if (resp.IsSuccessStatusCode)
                    itemList = await resp.Content.ReadAsAsync<ObservableCollection<DividendenAuswahlModel>>();
            }
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
