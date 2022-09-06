using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.Core.Validierungen;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using Data.Model.AktieModels;
using Data.Model.DepotModels;
using Data.Model.WertpapierModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Aktien.Logic.UI.WertpapierViewModels
{
    public class ReverseSplitEintragenViewModel : ViewModelValidate
    {
        private readonly ReverseSplitEintragenModel model;
        private int verhaeltnis;
        private int depotWertpapierID;

        public ReverseSplitEintragenViewModel()
        {
            Title = "Reverse-Split eintragen";
            model = new ReverseSplitEintragenModel();
            OpenAuswahlCommand = new RelayCommand(this.ExecuteOpenAuswahlCommand);
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Verhaeltnis = 1;
            ValidateNeueAktie("");
            depotWertpapierID = 0;

        }

        public int DepotWertpapierID
        {
            set
            {
                depotWertpapierID = value;
                LoadData(value);
            }
        }

        public async void LoadData(int id)
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/depot/Wertpapier/{id}");
                if (resp.IsSuccessStatusCode)
                {
                    Response<DepotWertpapierModel> Response = await resp.Content.ReadAsAsync<Response<DepotWertpapierModel>>();
                    model.AltWertpapier = Response.Data;
                }
                    
                RequestIsWorking = false;
            }
            RaisePropertyChanged("AlteAktieText");
        }

        public void BerechneWerte()
        {
            model.NeuWertpapier.Anzahl = Math.Round(model.AltWertpapier.Anzahl / verhaeltnis,3, MidpointRounding.AwayFromZero);
            model.NeuWertpapier.BuyIn = new KaufBerechnungen().BuyInAktieGekauft(0, 0, model.NeuWertpapier.Anzahl, (model.AltWertpapier.BuyIn * verhaeltnis), NeueAnzahl, 0, Data.Types.WertpapierTypes.OrderTypes.Normal);
            if (double.IsNaN(model.NeuWertpapier.BuyIn))
            {
                model.NeuWertpapier.BuyIn = 0;
            }

            RaisePropertyChanged(nameof(NeueAnzahl));
            RaisePropertyChanged(nameof(NeuerBuyIn));
        }

        #region Bindings

        public string AlteAktieText => model.AltWertpapier.Name;
        public string NeueAktieText => model.NeuWertpapier.Name;
        public double NeueAnzahl => model.NeuWertpapier.Anzahl;
        public double NeuerBuyIn => model.NeuWertpapier.BuyIn;
        public ICommand OpenAuswahlCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public int? Verhaeltnis
        {
            get => verhaeltnis;
            set
            {
                verhaeltnis = value.GetValueOrDefault(0);
                ValidateVerhaeltnis(verhaeltnis);
                BerechneWerte();
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        private void ExecuteOpenAuswahlCommand()
        {
            Messenger.Default.Send(new OpenWertpapierAuswahlMessage(OpenAktieMessageCallback) { WertpapierTypes = Data.Types.WertpapierTypes.WertpapierTypes.Aktie}, "ReverseSplitEintragen");
        }

        private bool CanExecuteSaveCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+ $"/api/depot/wertpapier/{depotWertpapierID}/ReverseSplit", model);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.aktien.ToString());
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.buysell.ToString());
                    Messenger.Default.Send<CloseViewMessage>(new CloseViewMessage(), "ReverseSplitEintragen");
                    SendInformationMessage("Gespeichert");
                }
                else if (resp.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    SendExceptionMessage("Aktie ist schon vorhanden");
                    return;
                }
            }     
        }

        #endregion

        #region Callbacks
        private async void OpenAktieMessageCallback(bool confirmed, int id)
        {
            if (confirmed)
            {
                if (GlobalVariables.ServerIsOnline)
                {
                    HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+"/api/Wertpapier/" + id.ToString());
                    if (resp.IsSuccessStatusCode)
                    {
                        var aktieResponse = await resp.Content.ReadAsAsync<Response<AktienModel>>();
                        model.NeuWertpapier.Name = aktieResponse.Data.Name;
                        model.NeuWertpapier.WertpapierID = aktieResponse.Data.ID;
                        model.NeuWertpapier.DepotID = 1;
                    }
                        
                }
                ValidateNeueAktie(model.NeuWertpapier.Name);
                BerechneWerte();
                RaisePropertyChanged("NeueAktieText");
            }
        }
        #endregion

        #region Validierungen
        private bool ValidateNeueAktie(string bezeichnung)
        {
            var Validierung = new SplitEintragenValidierung();

            bool isValid = Validierung.ValidateAktie(bezeichnung, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "NeueAktieText", validationErrors);
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            return isValid;
        }

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
