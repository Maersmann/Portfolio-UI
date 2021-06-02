using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
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

namespace Logic.UI.SteuerViewModels
{
    public class SteuerartStammdatenViewModel : ViewModelStammdaten<SteuerartModel>, IViewModelStammdaten
    {
        public SteuerartStammdatenViewModel()
        {
            Cleanup();
            Title = "Informationen Steuerart";
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + $"/api/Steuerarten", data);


                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), GetStammdatenTyp());
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
            LoadAktie = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/Steuerarten/" + id.ToString());
                if (resp.IsSuccessStatusCode)
                    data = await resp.Content.ReadAsAsync<SteuerartModel>();
            }
            Bezeichnung = data.Bezeichnung;
            SteuerberechnungZwischensumme = data.BerechnungZwischensumme;
            LoadAktie = false;
            state = State.Bearbeiten;
        }

        #region Bindings
        public string Bezeichnung
        {
            get { return this.data.Bezeichnung; }
            set
            {

                if (LoadAktie || !string.Equals(this.data.Bezeichnung, value))
                {
                    ValidateBezeichnung(value);
                    this.data.Bezeichnung = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public IEnumerable<SteuerberechnungZwischensumme> SteuerberechnungZwischensummes => Enum.GetValues(typeof(SteuerberechnungZwischensumme)).Cast<SteuerberechnungZwischensumme>();

        public SteuerberechnungZwischensumme SteuerberechnungZwischensumme
        {
            get { return data.BerechnungZwischensumme; }
            set
            {
                if (LoadAktie || (this.data.BerechnungZwischensumme != value))
                {
                    this.data.BerechnungZwischensumme = value;
                    this.RaisePropertyChanged();
                    
                }
            }
        }
        #endregion
        #region Validate
        private bool ValidateBezeichnung(String bezeichnung)
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
            data = new SteuerartModel();
            Bezeichnung = "";
        }

    }
}
