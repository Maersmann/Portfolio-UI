using Aktien.Data.Types;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.InterfaceViewModels;
using Base.Logic.Core;
using Base.Logic.Types;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using Data.DTO.SparplanDTOs;
using Data.Model.AktieModels;
using Data.Model.SparplanModels;
using Data.Types.SparplanTypes;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.SparplanViewModels
{
    public class SparplanStammdatenViewModel : ViewModelStammdaten<SparplanModel, StammdatenTypes>, IViewModelStammdaten
    {
        public SparplanStammdatenViewModel()
        {
            Title = "Informationen Sparplan";
            AuswahlCommand = new RelayCommand(() => ExcecuteAuswahlCommand());
            ValidateAktie("");
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + "/api/sparplan", 
                    new SparplanErstellenDTO
                    { 
                        Betrag = double.Parse(Data.Betrag),
                        ID = Data.ID,
                        Intervall = Data.Intervall,
                        StartDatum = Data.StartDatum,
                        WertpapierID = Data.WertpapierID
                    });
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
                }
                else if (resp.StatusCode.Equals(HttpStatusCode.Conflict))
                {
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                }
                else
                {
                    SendExceptionMessage("Sparplan konnte nicht gespeichert werden");
                    return;
                }
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.sparplan;
        public async void ZeigeStammdatenAn(int id)
        {
            RequestIsWorking = true;
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + "/api/sparplan/" + id.ToString());
                if (resp.IsSuccessStatusCode)
                {
                    Response<SparplanDTO> DTOResponse = await resp.Content.ReadAsAsync<Response<SparplanDTO>>();
                    Betrag = DTOResponse.Data.Betrag.ToString();
                    Data.ID = DTOResponse.Data.ID;
                    Data.WertpapierID = DTOResponse.Data.WertpapierID;
                    Data.WertpapierName = DTOResponse.Data.Wertpapier.Name;
                    Intervall = DTOResponse.Data.Intervall;
                    StartDatum = DTOResponse.Data.StartDatum;
                    ValidateAktie(Data.WertpapierName);
                    RaisePropertyChanged(nameof(WertpapierName));
                }
            }
            state = State.Bearbeiten;
            RaisePropertyChanged(nameof(CanWertpapierAuswaehlen));
            RequestIsWorking = false;
        }

        #region Bindings

        public ICommand AuswahlCommand { get; set; }
        public string WertpapierName => Data.WertpapierName;
        public bool CanWertpapierAuswaehlen => state.Equals(State.Neu);

        public string Betrag
        {
            get { return Data.Betrag; }
            set
            {
                if (RequestIsWorking || !string.Equals(Data.Betrag, value))
                {
                    if (!ValidateBetrag(value))
                    {
                        if (!value.Equals("0"))
                        {
                            Data.Betrag = "";
                            RaisePropertyChanged();
                        }
                        return;
                    }
                    Data.Betrag = value;
                    RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        
        public IEnumerable<SparplanIntervall> Intervalle => Enum.GetValues(typeof(SparplanIntervall)).Cast<SparplanIntervall>();

        public IEnumerable<SparplanStartDatum> StartDatums => Enum.GetValues(typeof(SparplanStartDatum)).Cast<SparplanStartDatum>();
        public SparplanIntervall Intervall
        {
            get { return Data.Intervall; }
            set
            {
                if (RequestIsWorking || (Data.Intervall != value))
                {
                    Data.Intervall = value;
                    RaisePropertyChanged();
                }
            }
        }

        public SparplanStartDatum StartDatum
        {
            get { return Data.StartDatum; }
            set
            {
                if (RequestIsWorking || (Data.StartDatum != value))
                {
                    Data.StartDatum = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Commands
        private void ExcecuteAuswahlCommand()
        {
            Messenger.Default.Send(new OpenWertpapierAuswahlMessage(OpenWertpapierMessageCallback), "SparplanStammdaten");
        }
        #endregion

        #region Callbacks
        private async void OpenWertpapierMessageCallback(bool confirmed, int id)
        {
            if (confirmed)
            {
                if (GlobalVariables.ServerIsOnline)
                {
                    HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + "/api/Wertpapier/" + id.ToString());
                    if (resp.IsSuccessStatusCode)
                    {
                        Response<AktienModel> aktieResponse = await resp.Content.ReadAsAsync<Response<AktienModel>>();
                        Data.WertpapierName = aktieResponse.Data.Name;
                        Data.WertpapierID = aktieResponse.Data.ID;
                        ValidateAktie(WertpapierName);
                    }

                }
                RaisePropertyChanged("WertpapierName");
            }
        }
        #endregion

        #region Validate
        private bool ValidateBetrag(string betrag)
        {
            BaseValidierung Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateBetrag(betrag, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Betrag", validationErrors);
            return isValid;
        }

        private bool ValidateAktie(string bezeichnung)
        {
            BaseValidierung Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(bezeichnung, "Aktie", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "WertpapierName", validationErrors);
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            state = State.Neu;
            Data = new SparplanModel { StartDatum = SparplanStartDatum.anfangDesMonats, Intervall = SparplanIntervall.monatlich };
            Betrag = "";
        }

    }
}
