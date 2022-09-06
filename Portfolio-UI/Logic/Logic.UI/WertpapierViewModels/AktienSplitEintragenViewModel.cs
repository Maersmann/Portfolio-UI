using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.Core.Validierungen;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using Data.Model.DepotModels;
using Data.Model.WertpapierModels;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.WertpapierViewModels
{
    public class AktienSplitEintragenViewModel : ViewModelValidate
    {
        private readonly AktienSplitEintragenDTO aktienSplit;
        private readonly AktienSplitModel model;


        public AktienSplitEintragenViewModel()
        {
            Title = "Aktien-Split eintragen";
            model = new AktienSplitModel(); 
            aktienSplit = new AktienSplitEintragenDTO { Verhaeltnis = 1, WertpapierID = 0 };
            SaveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            Verhaeltnis = 1;
        }

        public int DepotWertpapierID
        {
            set
            {
                aktienSplit.WertpapierID = value;
                LoadData(value);
            }
        }

        public async void LoadData(int id)
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/depot/Wertpapier/{id}");
                if (resp.IsSuccessStatusCode)
                {
                    Response<DepotWertpapierModel> Response = await resp.Content.ReadAsAsync<Response<DepotWertpapierModel>>();
                    model.WertpapierName = Response.Data.Name;
                    model.NeueAnzahl = Response.Data.Anzahl;
                    model.NeuerBuyIn = Response.Data.BuyIn;
                    model.AlteAnzahl = Response.Data.Anzahl;
                    model.AlterBuyIn = Response.Data.BuyIn;
                    model.DepotWertpapierID = Response.Data.ID;
                }

                RequestIsWorking = false;
            }
            RaisePropertyChanged("Model");
        }

        public void BerechneWerte()
        {
            model.NeueAnzahl = model.AlteAnzahl * aktienSplit.Verhaeltnis;
            model.NeuerBuyIn = new KaufBerechnungen().BuyInAktieGekauft(0, 0, model.NeueAnzahl, model.AlterBuyIn / aktienSplit.Verhaeltnis, model.NeueAnzahl, 0, OrderTypes.Normal);
            if (double.IsNaN(model.NeuerBuyIn))
            {
                model.NeuerBuyIn = 0;
            }

            RaisePropertyChanged(nameof(Model));
        }

        #region Bindings

        public AktienSplitModel Model => model;
        public ICommand OpenAuswahlCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public int? Verhaeltnis
        {
            get => aktienSplit.Verhaeltnis;
            set
            {
                aktienSplit.Verhaeltnis = value.GetValueOrDefault(0);
                ValidateVerhaeltnis(aktienSplit.Verhaeltnis);
                BerechneWerte();
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        private bool CanExecuteSaveCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + $"/api/depot/wertpapier/{model.DepotWertpapierID}/AktienSplit", aktienSplit);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send(new AktualisiereViewMessage(), StammdatenTypes.aktien.ToString());
                    Messenger.Default.Send(new AktualisiereViewMessage(), StammdatenTypes.buysell.ToString());
                    Messenger.Default.Send(new CloseViewMessage(), "AktienSplitEintragen");
                    SendInformationMessage("Gespeichert");
                }
                else
                {
                    SendExceptionMessage("Aktien-Split konnte nicht gespeichert werden.");
                    return;
                }
            }
        }

        #endregion


        #region Validierungen

        private bool ValidateVerhaeltnis(int verhaeltnis)
        {
            var Validierung = new SplitEintragenValidierung();

            bool isValid = Validierung.ValidateAnzahl(verhaeltnis, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Verhaeltnis", validationErrors);
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            return isValid;
        }

        #endregion
    }
}
