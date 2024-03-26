using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aktien.Logic.Core.Validierung;
using System.Runtime.CompilerServices;
using Prism.Commands;
using System.Windows.Input;
using Aktien.Logic.Messages.Base;
using Aktien.Data.Types;
using Base.Logic.ViewModels;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.UI.InterfaceViewModels;
using Aktien.Logic.Core;
using System.Net.Http;
using Aktien.Data.Types.WertpapierTypes;
using System.Net;
using Data.Model.AktieModels;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;
using Base.Logic.Wrapper;

namespace Aktien.Logic.UI.AktieViewModels
{
    public class AktieStammdatenViewModel : ViewModelStammdaten<AktienModel, StammdatenTypes>, IViewModelStammdaten
    {
        public AktieStammdatenViewModel()
        {
            Title = "Informationen Aktie";
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+"/api/Wertpapier", Data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                     WeakReferenceMessenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp().ToString());
                     WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());
                }
                else if((int)resp.StatusCode == 904)
                {
                    SendExceptionMessage("Aktie ist schon vorhanden");
                    return;
                }
                else
                {
                    SendExceptionMessage("Aktie konnte nicht gespeichert werden");
                    return;
                }
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.aktien;
        public async void ZeigeStammdatenAn(int id) 
        {
            RequestIsWorking = true;
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + "/api/Wertpapier/" + id.ToString());
                if (resp.IsSuccessStatusCode)
                {
                    Response = await resp.Content.ReadAsAsync<Response<AktienModel>>();
                }       
            }
            WKN = Data.WKN;
            Name = Data.Name;
            ISIN = Data.ISIN;
            RequestIsWorking = false;
            state = State.Bearbeiten;
            OnPropertyChanged(nameof(ISIN_isEnabled));
            RequestIsWorking = false;
        }


        public bool ISIN_isEnabled { get{ return state == State.Neu; } }
        public string ISIN
        {
            get { return Data.ISIN; }
            set
            {

                if (RequestIsWorking || !string.Equals(Data.ISIN, value))
                {
                    ValidateISIN(value);
                    Data.ISIN = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string Name{
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
        public string WKN
        {
            get { return Data.WKN; }
            set
            {

                if (RequestIsWorking || !string.Equals(Data.WKN, value))
                {
                    Data.WKN = value;
                    OnPropertyChanged();
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

        private bool ValidateISIN(String isin)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(isin, "Die ISIN", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "ISIN", validationErrors);
            return isValid;
        }
        #endregion

        protected override void OnActivated()
        {
            state = State.Neu;
            Data = new AktienModel();
            ISIN = "";
            Name = "";
            WKN = "";
        }

    }
}
