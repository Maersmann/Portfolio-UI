using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.Core.Validierungen;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
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
        private ReverseSplitEintragenModel model;
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
                HttpResponseMessage resp = await Client.GetAsync($"https://localhost:5001/api/depot/Wertpapier/{id}");
                if (resp.IsSuccessStatusCode)
                    model.AltWertpapier = await resp.Content.ReadAsAsync<DepotWertpapierModel>();
            }             
            this.RaisePropertyChanged("AlteAktieText");
        }

        public void BerechneWerte()
        {
            model.NeuWertpapier.Anzahl = Math.Round(model.AltWertpapier.Anzahl / verhaeltnis,3, MidpointRounding.AwayFromZero);
            model.NeuWertpapier.BuyIn = new KaufBerechnungen().BuyInAktieGekauft(0, 0, model.NeuWertpapier.Anzahl, (model.AltWertpapier.BuyIn * verhaeltnis), NeueAnzahl, 0, Data.Types.WertpapierTypes.OrderTypes.Normal);
            if (Double.IsNaN(model.NeuWertpapier.BuyIn)) model.NeuWertpapier.BuyIn = 0;
            this.RaisePropertyChanged(nameof(NeueAnzahl));
            this.RaisePropertyChanged(nameof(NeuerBuyIn));
        }

        #region Bindings

        public string AlteAktieText => model.AltWertpapier.Name;
        public string NeueAktieText => model.NeuWertpapier.Name;
        public Double NeueAnzahl => model.NeuWertpapier.Anzahl;
        public Double NeuerBuyIn => model.NeuWertpapier.BuyIn;
        public ICommand OpenAuswahlCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public int? Verhaeltnis
        {
            get => this.verhaeltnis;
            set
            {
                verhaeltnis = value.GetValueOrDefault(0);
                ValidateVerhaeltnis(verhaeltnis);
                BerechneWerte();
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        private void ExecuteOpenAuswahlCommand()
        {
            Messenger.Default.Send<OpenWertpapierAuswahlMessage>(new OpenWertpapierAuswahlMessage(OpenAktieMessageCallback) { WertpapierTypes = Data.Types.WertpapierTypes.WertpapierTypes.Aktie}, "ReverseSplitEintragen");
        }

        private bool CanExecuteSaveCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.PostAsJsonAsync($"https://localhost:5001/api/depot/wertpapier/{depotWertpapierID}/ReverseSplit", model);


                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.aktien);
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.buysell);
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
                    HttpResponseMessage resp = await Client.GetAsync("https://localhost:5001/api/Wertpapier/" + id.ToString());
                    if (resp.IsSuccessStatusCode)
                    {
                        var aktie = await resp.Content.ReadAsAsync<AktienModel>();
                        model.NeuWertpapier.Name = aktie.Name;
                        model.NeuWertpapier.WertpapierID = aktie.ID;
                        model.NeuWertpapier.DepotID = 1;
                    }
                        
                }
                ValidateNeueAktie(model.NeuWertpapier.Name);
                BerechneWerte();
                this.RaisePropertyChanged("NeueAktieText");
            }
        }
        #endregion

        #region Validierungen
        private bool ValidateNeueAktie(string bezeichnung)
        {
            var Validierung = new ReverseSplitEintragenValidierung();

            bool isValid = Validierung.ValidateAktie(bezeichnung, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "NeueAktieText", validationErrors);
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            return isValid;
        }

        private bool ValidateVerhaeltnis(int verhaeltnis)
        {
            var Validierung = new ReverseSplitEintragenValidierung();

            bool isValid = Validierung.ValidateAnzahl(verhaeltnis, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Verhaeltnis", validationErrors);
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            return isValid;
        }

        #endregion
    }
}
