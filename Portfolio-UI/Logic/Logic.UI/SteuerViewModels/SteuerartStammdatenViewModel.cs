using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Base.Logic.ViewModels;
using Aktien.Logic.UI.InterfaceViewModels;
using Data.Model.SteuerModels;
using Data.Types.SteuerTypes;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;
using Base.Logic.Wrapper;

namespace Logic.UI.SteuerViewModels
{
    public class SteuerartStammdatenViewModel : ViewModelStammdaten<SteuerartModel, StammdatenTypes>, IViewModelStammdaten
    {
        public SteuerartStammdatenViewModel()
        {
            Title = "Informationen Steuerart";
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + $"/api/Steuerarten", Data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
                    Messenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());
                }
                else
                {
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                    return;
                }
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.steuerart;
        public async void ZeigeStammdatenAn(int id)
        {
            RequestIsWorking = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/Steuerarten/" + id.ToString());
                if (resp.IsSuccessStatusCode)
                    Response = await resp.Content.ReadAsAsync<Response<SteuerartModel>>();
            }
            Bezeichnung = Data.Bezeichnung;
            SteuerberechnungZwischensumme = Data.BerechnungZwischensumme;
            RequestIsWorking = false;
            state = State.Bearbeiten;
        }

        #region Bindings
        public string Bezeichnung
        {
            get => Data.Bezeichnung;
            set
            {

                if (RequestIsWorking || !string.Equals(Data.Bezeichnung, value))
                {
                    ValidateBezeichnung(value);
                    Data.Bezeichnung = value;
                    RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public IEnumerable<SteuerberechnungZwischensumme> SteuerberechnungZwischensummes => Enum.GetValues(typeof(SteuerberechnungZwischensumme)).Cast<SteuerberechnungZwischensumme>();

        public SteuerberechnungZwischensumme SteuerberechnungZwischensumme
        {
            get => Data.BerechnungZwischensumme;
            set
            {
                if (RequestIsWorking || (Data.BerechnungZwischensumme != value))
                {
                    Data.BerechnungZwischensumme = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion
       
        #region Validate
        private bool ValidateBezeichnung(string bezeichnung)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(bezeichnung, "Die Bezeichnung", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Bezeichnung", validationErrors);
            return isValid;
        }

        #endregion

        public override void Cleanup()
        {
            state = State.Neu;
            Data = new SteuerartModel();
            Bezeichnung = "";
        }

    }
}
