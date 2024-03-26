using Aktien.Data.Types;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
using Base.Logic.ViewModels;

using CommunityToolkit.Mvvm.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Aktien.Logic.Core.DepotLogic;
using System.Net;
using Aktien.Logic.Core;
using System.Net.Http;
using Data.Model.DepotModels;
using Data.Model.OptionenModels;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Wrapper;
using CommunityToolkit.Mvvm.Input;

namespace Aktien.Logic.UI.OptionenViewModels
{
    public class DatenAnpassungViewModel : ViewModelBasis
    {
        private WertpapierBuyInModel BuyInModel;
        public DatenAnpassungViewModel()
        {
            Title = "Anpassungen";
            SpeicherBuyInCommand = new DelegateCommand( ExecuteSpeicherBuyInCommand, CanExecuteSpeicherBuyInCommand);
            AuswahlBuyInAktie = new RelayCommand(() => ExecuteAuswahlBuyInAktie());
            BuyInModel = new WertpapierBuyInModel { AlterBuyIn = 0, NeuerBuyIn = 0, DepotWertpapierID = 0, WertpapierName = "<<Nicht ausgewählt>>" };
            OnPropertyChanged(nameof(WertpapierBuyInModel));
        }

        #region Bindings
        public ICommand SpeicherBuyInCommand { get; set; }
        public ICommand AuswahlBuyInAktie { get; set; }
        public WertpapierBuyInModel WertpapierBuyInModel { get => BuyInModel; }
        #endregion

        #region Commands
        private async void ExecuteSpeicherBuyInCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + "/api/depot/wertpapier", new DepotWertpapierModel
                {
                    Anzahl = BuyInModel.Anzahl,
                    BuyIn = BuyInModel.NeuerBuyIn,
                    DepotID = BuyInModel.DepotID,
                    ID = BuyInModel.DepotWertpapierID,
                    WertpapierID = BuyInModel.WertpapierID
                });
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    SendInformationMessage("Erledigt");
                     WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage(), StammdatenTypes.buysell.ToString());
                    BuyInModel = new WertpapierBuyInModel { AlterBuyIn = 0, NeuerBuyIn = 0, DepotWertpapierID = 0, WertpapierName = "<<Nicht ausgewählt>>" };
                    OnPropertyChanged("WertpapierBuyInModel");
                    ((DelegateCommand)SpeicherBuyInCommand).RaiseCanExecuteChanged();
                }
                else if (resp.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    SendExceptionMessage("Aktie ist schon vorhanden");
                    return;
                }
            }
        }
        private bool CanExecuteSpeicherBuyInCommand()
        {
            return BuyInModel.DepotWertpapierID != 0;
        }
        private void ExecuteAuswahlBuyInAktie()
        {
             WeakReferenceMessenger.Default.Send(new OpenWertpapierAuswahlMessage(OpenWertpapierAuswahlMessageCallback), "DatenAnpassung");
        }
        #endregion

        #region Callbacks
        private async void OpenWertpapierAuswahlMessageCallback(bool confirmed, int id)
        {
            if (confirmed)
            {
                if (GlobalVariables.ServerIsOnline)
                {
                    RequestIsWorking = true;
                    SetConnection();
                    HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/depot/Wertpapier/{id}/BuyInNeuBerechnen");
                    if (resp.IsSuccessStatusCode)
                    {
                        Response<WertpapierBuyInModel> BuyInModelResponse = await resp.Content.ReadAsAsync<Response<WertpapierBuyInModel>>();
                        BuyInModel = BuyInModelResponse.Data;
                        OnPropertyChanged("WertpapierBuyInModel");
                        ((DelegateCommand)SpeicherBuyInCommand).RaiseCanExecuteChanged();
                    }
                    RequestIsWorking = false;
                        
                }
            }
        } 
        #endregion
    }
}
