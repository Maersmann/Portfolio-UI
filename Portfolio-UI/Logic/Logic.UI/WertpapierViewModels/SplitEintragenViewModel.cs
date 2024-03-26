using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Core.Validierungen;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using Data.Model.DepotModels;
using Data.Model.WertpapierModels;
using CommunityToolkit.Mvvm.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.WertpapierViewModels
{
    public class SplitEintragenViewModel : ViewModelValidate
    {
        private readonly SplitEintragenDTO split;
        private readonly SplitModel model;


        public SplitEintragenViewModel()
        {
            Title = "Split eintragen";
            model = new SplitModel { Datum = DateTime.Now };
            split = new SplitEintragenDTO { Verhaeltnis = 1, WertpapierID = 0, Datum = DateTime.Now };
            SaveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            Verhaeltnis = 1;
        }

        public int DepotWertpapierID
        {
            set
            {
                split.WertpapierID = value;
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
            OnPropertyChanged("Model");
        }

        public void BerechneWerte()
        {
            model.NeueAnzahl = model.AlteAnzahl * split.Verhaeltnis;
            model.NeuerBuyIn = new KaufBerechnungen().BuyInAktieGekauft(0, 0, model.NeueAnzahl, model.AlterBuyIn / split.Verhaeltnis, model.NeueAnzahl, 0, OrderTypes.Normal);
            if (double.IsNaN(model.NeuerBuyIn))
            {
                model.NeuerBuyIn = 0;
            }

            OnPropertyChanged(nameof(Model));
        }

        #region Bindings

        public SplitModel Model => model;
        public ICommand OpenAuswahlCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public int? Verhaeltnis
        {
            get => split.Verhaeltnis;
            set
            {
                split.Verhaeltnis = value.GetValueOrDefault(0);
                ValidateVerhaeltnis(split.Verhaeltnis);
                BerechneWerte();
                OnPropertyChanged();
            }
        }

        public DateTime? Datum
        {
            get => split.Datum;
            set
            {
                if (RequestIsWorking || !Equals(split.Datum, value))
                {
                    ValidateDatum(value, nameof(Datum));
                    split.Datum = value;
                    OnPropertyChanged();
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
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
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + $"/api/depot/wertpapier/{model.DepotWertpapierID}/Split", split);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                     WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage(), StammdatenTypes.aktien.ToString());
                     WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage(), StammdatenTypes.buysell.ToString());
                     WeakReferenceMessenger.Default.Send(new CloseViewMessage(), "SplitEintragen");
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

        private bool ValidateDatum(DateTime? datun, string fieldname)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateDatum(datun, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
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
