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
using CommunityToolkit.Mvvm.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Windows.Input;
using Data.Model.ZinsenModels;
using Logic.Messages.SteuernMessages;
using Data.Model.SteuerModels;
using Data.DTO.ZinsenDTOs;
using Logic.Core.SteuernLogic;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Linq;

namespace Logic.UI.ZinsenViewModels
{
    public class ZinsenEintragenViewModel : ViewModelStammdaten<ZinsenEintragenModel, StammdatenTypes>, IViewModelStammdaten
    {
        private IEnumerable<string> monateNamen;
        public ZinsenEintragenViewModel()
        {
            Title = "Zinsen erhalten";
            OpenSteuernCommand = new RelayCommand(ExecuteOpenSteuernCommand);
        }

        private void BerechneGesamtWerte()
        {
            var steuer = Data.Steuer;
            Data.Steuer.BetragVorZwischensumme = new SteuerBerechnen().SteuernVorZwischensumme(steuer.Steuern);
            Data.Steuer.BetragNachZwischensumme = new SteuerBerechnen().SteuernNachZwischensumme(steuer.Steuern);

            Data.SteuernGesamt = Math.Abs(Math.Round(Data.Steuer.BetragNachZwischensumme + Data.Steuer.BetragVorZwischensumme, 2, MidpointRounding.AwayFromZero)).ToString();

            Data.Erhalten = Math.Round( Double.Parse(Data.Gesamt) - Double.Parse(Data.SteuernGesamt), 2, MidpointRounding.AwayFromZero).ToString();

            OnPropertyChanged(nameof(SteuernGesamt));
            OnPropertyChanged(nameof(Erhalten));
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp;
                if (state.Equals(State.Neu))
                {
                    resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + "/api/zinsen",
                        new ZinsenEintragenDTO
                        {
                            DurchschnittlicherKontostand = Double.Parse(Data.DurchschnittlicherKontostand),
                            Gesamt = Double.Parse(Data.Gesamt),
                            Prozent = Double.Parse(Data.Prozent),
                            ID = 0,
                            ErhaltenAm = Data.ErhaltenAm.Value,
                            Erhalten = Double.Parse(Data.Erhalten),
                            Steuer = Data.Steuer,
                            Abrechnungsmonat = new DateTime( int.Parse(Data.Jahr), DateTime.ParseExact(Data.Monat, "MMMM", CultureInfo.CurrentCulture).Month, 1)
                        }) ;
                }
                else
                {
                    throw new NotImplementedException();
                }
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                     WeakReferenceMessenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp().ToString());
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
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.zinsenErhalten;
        public void ZeigeStammdatenAn(int id)
        {
            throw new NotImplementedException();
        }

        #region Bindings

        public ICommand OpenSteuernCommand { get; set; }

        public string Gesamt
        {
            get { return Data.Gesamt; }
            set
            {
                if (RequestIsWorking || !string.Equals(Data.Gesamt, value))
                {
                    if (!ValidateGesamt(value))
                    {
                        if (!value.Equals("0"))
                        {
                            Data.Gesamt = "";
                            OnPropertyChanged();
                        }
                        return;
                    }
                    Data.Gesamt = value;
                    BerechneGesamtWerte();
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Kontostand
        {
            get { return Data.DurchschnittlicherKontostand; }
            set
            {
                if (RequestIsWorking || !string.Equals(Data.DurchschnittlicherKontostand, value))
                {
                    if (!ValidateKontostand(value))
                    {
                        if (!value.Equals("0"))
                        {
                            Data.DurchschnittlicherKontostand = "";
                            OnPropertyChanged();
                        }
                        return;
                    }
                    Data.DurchschnittlicherKontostand = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Prozent
        {
            get { return Data.Prozent; }
            set
            {
                if (RequestIsWorking || !string.Equals(Data.Prozent, value))
                {
                    if (!ValidateProzent(value))
                    {
                        if (!value.Equals("0"))
                        {
                            Data.Prozent = "";
                            OnPropertyChanged();
                        }
                        return;
                    }
                    Data.Prozent = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public DateTime? ErhaltenAm
        {
            get => Data.ErhaltenAm.Equals(DateTime.MinValue) ? null : Data.ErhaltenAm;
            set
            {
                if (RequestIsWorking || !Equals(Data.ErhaltenAm, value))
                {
                    ValidateDatum(value, nameof(ErhaltenAm));
                    Data.ErhaltenAm = value;
                    OnPropertyChanged();
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public string SteuernGesamt => Data.SteuernGesamt;
        public string Erhalten => Data.Erhalten;


        public IEnumerable<string> Monate => monateNamen;

        public string Monat
        {
            get { return Data.Monat; }
            set
            {
                if (RequestIsWorking || (Data.Monat != value))
                {
                    Data.Monat = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Jahr
        {
            get { return Data.Jahr; }
            set
            {
                if (RequestIsWorking || !string.Equals(Data.Jahr, value))
                {
                    if (!ValidateJahr(value))
                    {
                        if (!value.Equals("0"))
                        {
                            Data.Jahr = "";
                            OnPropertyChanged();
                        }
                        return;
                    }
                    Data.Jahr = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Commands

        private void ExecuteOpenSteuernCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenSteuernUebersichtMessage(OpenSteuernUebersichtMessageCallback, Data.Steuer.Steuern), "ZinsenEintragen");
        }

        private void OpenSteuernUebersichtMessageCallback(bool confirmed, IList<SteuerModel> steuern)
        {
            if (confirmed)
            {
                Data.Steuer.Steuern = steuern;
                BerechneGesamtWerte();
            }
        }

        #endregion

        #region Validate
        private bool ValidateGesamt(string betrag)
        {
            BaseValidierung Validierung = new();

            bool isValid = Validierung.ValidateZahl(betrag, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Gesamt", validationErrors);
            return isValid;
        }

        private bool ValidateKontostand(string kontostand)
        {
            BaseValidierung Validierung = new();

            bool isValid = Validierung.ValidateZahl(kontostand, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Kontostand", validationErrors);
            return isValid;
        }

        private bool ValidateProzent(string prozent)
        {
            BaseValidierung Validierung = new();

            bool isValid = Validierung.ValidateZahl(prozent, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Prozent", validationErrors);
            return isValid;
        }

        private bool ValidateJahr(string jahr)
        {
            BaseValidierung Validierung = new();

            bool isValid = Validierung.ValidateZahl(jahr, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Jahr", validationErrors);
            return isValid;
        }

        private bool ValidateDatum(DateTime? datun, string fieldname)
        {
            BaseValidierung Validierung = new();

            bool isValid = Validierung.ValidateDatum(datun, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }

        #endregion

        protected override void OnActivated()
        {
            OnPropertyChanged(nameof(Monate));
            state = State.Neu;
            Data = new ZinsenEintragenModel { };
            Kontostand = "";
            Prozent = "";
            Gesamt = "";
            ErhaltenAm = DateTime.Now;
            Monat = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            Jahr = DateTime.Now.Year.ToString();
            monateNamen = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12);
        }

    }
}
