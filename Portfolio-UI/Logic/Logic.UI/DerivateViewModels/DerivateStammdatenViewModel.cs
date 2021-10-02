using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Base.Logic.ViewModels;
using Aktien.Logic.UI.InterfaceViewModels;
using Data.Model.DerivateModels;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;

namespace Aktien.Logic.UI.DerivateViewModels
{
    public class DerivateStammdatenViewModel : ViewModelStammdaten<DerivateModel, StammdatenTypes>,IViewModelStammdaten
    {

        public DerivateStammdatenViewModel()
        {
            Cleanup();
            Title = "Informationen Derivate";
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+"/api/Wertpapier", data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
                    Messenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());
                }
                else if ((int)resp.StatusCode == 904)
                {
                    SendExceptionMessage("Derivate ist schon vorhanden");
                    return;
                }
                else
                {
                    SendExceptionMessage("Derivate konnte nicht gespeichert werden.");
                    return;
                }
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.derivate;

        public async void ZeigeStammdatenAn(int id)
        {
            RequestIsWorking = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/Wertpapier/{id}");
                if (resp.IsSuccessStatusCode)
                    data = await resp.Content.ReadAsAsync<DerivateModel>();
            }
            WKN = data.WKN;
            Name = data.Name;
            ISIN = data.ISIN;
            RequestIsWorking = false;
            state = State.Bearbeiten;
            RaisePropertyChanged(nameof(ISIN_isEnabled));
        }


        public bool ISIN_isEnabled { get { return state == State.Neu; } }

        #region Bindings   
        public string ISIN
        {
            get { return data.ISIN; }
            set
            {

                if (RequestIsWorking || !string.Equals(data.ISIN, value))
                {
                    ValidateISIN(value);
                    data.ISIN = value;
                    RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string Name
        {
            get { return data.Name; }
            set
            {
                if (RequestIsWorking || !string.Equals(data.Name, value))
                {
                    ValidateName(value);
                    data.Name = value;
                    RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string WKN
        {
            get { return data.WKN; }
            set
            {

                if (RequestIsWorking || !string.Equals(data.WKN, value))
                {
                    data.WKN = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Validate
        private bool ValidateName(String name)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(name, "Der Name", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Name", validationErrors);
            return isValid;
        }

        private bool ValidateISIN(String isin)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(isin, "Die ISIN", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "ISIN", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            state = State.Neu;
            data = new DerivateModel();
            ISIN = "";
            Name = "";
            WKN = "";
        }
    }
}
