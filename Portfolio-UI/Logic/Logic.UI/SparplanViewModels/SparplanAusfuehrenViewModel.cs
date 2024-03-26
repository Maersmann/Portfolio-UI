using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
using Data.DTO.SparplanDTOs;
using Data.Model.SparplanModels;
using CommunityToolkit.Mvvm.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.SparplanViewModels
{
    public class SparplanAusfuehrenViewModel : ViewModelValidate
    {
        private readonly SparplanAusfuehrenModel model;

        public SparplanAusfuehrenViewModel()
        {
            Title = "Sparplan ausführen";
            model = new SparplanAusfuehrenModel { Anzahl = "" };
            SaveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
        }

        public void SetzInformationen(SparplanAusfuehrenUebersichtModel sparplanAusfuehrenUebersicht)
        {
            model.Betrag = sparplanAusfuehrenUebersicht.Betrag.ToString();
            model.ID = sparplanAusfuehrenUebersicht.ID;
            model.WertpapierName = sparplanAusfuehrenUebersicht.Wertpapier.Name;
            model.NaechsteAusfuehrung = sparplanAusfuehrenUebersicht.NaechsteAusfuehrung;
            ValidateBetrag(model.Betrag);
            ValidateAnzahl(model.Anzahl);
            OnPropertyChanged(nameof(WertpapierName));
            OnPropertyChanged(nameof(Betrag));
            OnPropertyChanged(nameof(Anzahl));
            BerechneWerte();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public void BerechneWerte()
        {
            
            model.BuyIn = !double.TryParse(model.Betrag, out double Betrag) || !double.TryParse(model.Anzahl, out double Anzahl)
                ? 0
                : Anzahl == 0 ? 0 : Math.Round(Betrag / Anzahl, 3, MidpointRounding.AwayFromZero);
            OnPropertyChanged(nameof(BuyIn));
        }

        #region Bindings

        public ICommand SaveCommand { get; set; }
        public string WertpapierName => model.WertpapierName;
        public double BuyIn => model.BuyIn;

        public string Betrag
        {
            get { return model.Betrag; }
            set
            {
                if (RequestIsWorking || !string.Equals(model.Betrag, value))
                {
                    if (!ValidateBetrag(value))
                    {
                        if (!value.Equals("0"))
                        {
                            model.Betrag = "";
                            OnPropertyChanged();
                        }
                        return;
                    }
                    model.Betrag = value;
                    BerechneWerte();
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string Anzahl
        {
            get { return model.Anzahl; }
            set
            {
                if (RequestIsWorking || !string.Equals(model.Anzahl, value))
                {
                    if (!ValidateAnzahl(value))
                    {
                        if (!value.Equals("0"))
                        {
                            model.Anzahl = "";
                            OnPropertyChanged();
                        }
                        return;
                    }
                    model.Anzahl = value;
                    BerechneWerte();
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
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
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + $"/api/sparplan/ausfuehren", 
                    new SparplanAusfuehrenDTO 
                    { 
                        Anzahl = double.Parse(model.Anzahl),
                        Betrag = double.Parse(model.Betrag),
                        SparplanID = model.ID
                    });
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                     WeakReferenceMessenger.Default.Send(new CloseViewMessage(), "SparplanAusfuehren");
                    SendInformationMessage("Gespeichert");
                     WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage(), StammdatenTypes.sparplan.ToString());
                }
                else
                {
                    SendExceptionMessage("Sparplan konnte nicht ausgeführt werden.");
                    return;
                }
            }
        }

        #endregion


        #region Validierungen
        private bool ValidateBetrag(string betrag)
        {
            BaseValidierung Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateBetrag(betrag, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Betrag", validationErrors);
            return isValid;
        }

        private bool ValidateAnzahl(string anzahl)
        {
            BaseValidierung Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateAnzahl(anzahl, out ICollection<string> validationErrors, true);

            AddValidateInfo(isValid, "Anzahl", validationErrors);
            return isValid;
        }
        #endregion
    }
}

