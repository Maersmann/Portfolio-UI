using Aktien.Data.Types;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.InterfaceViewModels;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using CommunityToolkit.Mvvm.Messaging;
using Data.Model.AktieModels;
using Data.Model.CryptoModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logic.UI.CryptoViewModels
{
    public class CryptoStammdatenViewModel : ViewModelStammdaten<CryptoModel, StammdatenTypes>, IViewModelStammdaten
    {
        public CryptoStammdatenViewModel()
        {
            Title = "Informationen Crypto";
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + "/api/Wertpapier", Data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    WeakReferenceMessenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp().ToString());
                    WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());
                }
                else if ((int)resp.StatusCode == 904)
                {
                    SendExceptionMessage("Crypto ist schon vorhanden");
                    return;
                }
                else
                {
                    SendExceptionMessage("Crypto konnte nicht gespeichert werden");
                    return;
                }
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.crypto;
        public async void ZeigeStammdatenAn(int id)
        {
            RequestIsWorking = true;
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + "/api/Wertpapier/" + id.ToString());
                if (resp.IsSuccessStatusCode)
                {
                    Response = await resp.Content.ReadAsAsync<Response<CryptoModel>>();
                }
            }
            Name = Data.Name;
            RequestIsWorking = false;
            state = State.Bearbeiten;
            RequestIsWorking = false;
        }


        public string Name
        {
            get { return Data.Name; }
            set
            {
                if (RequestIsWorking || !string.Equals(Data.Name, value))
                {
                    ValidateName(value);
                    Data.Name = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }


        #region Validate
        private bool ValidateName(String name)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(name, "Der Name", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Name", validationErrors);
            return isValid;
        }

        #endregion

        protected override void OnActivated()
        {
            state = State.Neu;
            Data = new CryptoModel();
            Name = "";
        }

    }
}
