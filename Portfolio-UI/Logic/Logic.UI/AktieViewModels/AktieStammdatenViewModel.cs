using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
using Aktien.Logic.UI.BaseViewModels;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.UI.InterfaceViewModels;
using Aktien.Logic.Core;
using System.Net.Http;
using Aktien.Data.Types.WertpapierTypes;
using System.Net;
using Data.Model.AktieModels;

namespace Aktien.Logic.UI.AktieViewModels
{
    public class AktieStammdatenViewModel : ViewModelStammdaten<AktienModel>, IViewModelStammdaten
    {
        public AktieStammdatenViewModel()
        {
            Cleanup();
            Title = "Informationen Aktie";
        }


        protected async override void ExecuteSaveCommand()
        {   
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+"/api/Wertpapier", data);


                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), GetStammdatenTyp());
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
            LoadAktie = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+"/api/Wertpapier/"+id.ToString());
                if (resp.IsSuccessStatusCode)
                    data = await resp.Content.ReadAsAsync<AktienModel>();
            }
            WKN = data.WKN;
            Name = data.Name;
            ISIN = data.ISIN;
            LoadAktie = false;
            state = State.Bearbeiten;
            this.RaisePropertyChanged(nameof(ISIN_isEnabled));           
        }


        public bool ISIN_isEnabled { get{ return state == State.Neu; } }
        public string ISIN
        {
            get { return this.data.ISIN; }
            set
            {

                if (LoadAktie || !string.Equals(this.data.ISIN, value))
                {
                    ValidateISIN(value);
                    this.data.ISIN = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string Name{
            get { return this.data.Name; }
            set
            {               
                if (LoadAktie || !string.Equals(this.data.Name, value))
                {
                    ValidateName(value);
                    this.data.Name = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string WKN
        {
            get { return this.data.WKN; }
            set
            {

                if (LoadAktie || !string.Equals(this.data.WKN, value))
                {
                    this.data.WKN = value;
                    this.RaisePropertyChanged();
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

        public override void Cleanup()
        {
            state = State.Neu;
            data = new AktienModel();
            ISIN = "";
            Name = "";
            WKN = "";
        }

    }
}
