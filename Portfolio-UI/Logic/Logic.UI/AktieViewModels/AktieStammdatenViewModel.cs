﻿using GalaSoft.MvvmLight;
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

namespace Aktien.Logic.UI.AktieViewModels
{
    public class AktieStammdatenViewModel : ViewModelStammdaten<AktienModel, StammdatenTypes>, IViewModelStammdaten
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
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+"/api/Wertpapier", data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
                    Messenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp());
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
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+"/api/Wertpapier/"+id.ToString());
                if (resp.IsSuccessStatusCode)
                    data = await resp.Content.ReadAsAsync<AktienModel>();
                RequestIsWorking = false;
            }
            WKN = data.WKN;
            Name = data.Name;
            ISIN = data.ISIN;
            RequestIsWorking = false;
            state = State.Bearbeiten;
            RaisePropertyChanged(nameof(ISIN_isEnabled));       
        }


        public bool ISIN_isEnabled { get{ return state == State.Neu; } }
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
        public string Name{
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
