using Aktien.Data.Types;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using Aktien.Logic.UI.OptionenViewModels.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
            this.RaisePropertyChanged("WertpapierBuyInModel");
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
                HttpResponseMessage resp = await Client.PostAsJsonAsync("https://localhost:5001/api/depot/wertpapier", new DepotWertpapierModel { 
                    Anzahl = BuyInModel.Anzahl, 
                    BuyIn = BuyInModel.NeuerBuyIn, 
                    DepotID = BuyInModel.DepotID, 
                    ID = BuyInModel.DepotWertpapierID, 
                    WertpapierID = BuyInModel.WertpapierID 
                });

                if (resp.IsSuccessStatusCode)
                {
                    SendInformationMessage("Erledigt");
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.buysell);
                    BuyInModel = new WertpapierBuyInModel { AlterBuyIn = 0, NeuerBuyIn = 0, DepotWertpapierID = 0, WertpapierName = "<<Nicht ausgewählt>>" };
                    this.RaisePropertyChanged("WertpapierBuyInModel");
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
            Messenger.Default.Send<OpenWertpapierAuswahlMessage>(new OpenWertpapierAuswahlMessage(OpenOpenWertpapierAuswahlMessageCallback), "DatenAnpassung");
        }
        #endregion

        #region Callbacks
        private async void OpenOpenWertpapierAuswahlMessageCallback(bool confirmed, int id)
        {
            if (confirmed)
            {
                if (GlobalVariables.ServerIsOnline)
                {
                    HttpResponseMessage resp = await Client.GetAsync($"https://localhost:5001/api/depot/Wertpapier/{id}/BuyInNeuBerechnen");
                    if (resp.IsSuccessStatusCode)
                    {
                        BuyInModel = await resp.Content.ReadAsAsync<WertpapierBuyInModel>();
                        this.RaisePropertyChanged("WertpapierBuyInModel");
                        ((DelegateCommand)SpeicherBuyInCommand).RaiseCanExecuteChanged();
                    }
                        
                }
            }
        } 
        #endregion
    }
}
