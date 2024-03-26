using Aktien.Data.Types;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using Data.DTO.DepotDTOs;
using Data.Model.DepotModels;
using Data.Model.SteuerModels;
using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.SteuernMessages;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Logic.UI.DepotViewModels
{
    public class ErhalteneDividendeEintragenViewModel : ViewModelStammdaten<ErhalteneDividendeEintragenModel, StammdatenTypes>
    {
        public ErhalteneDividendeEintragenViewModel()
        {
            OpenSteuernCommand = new RelayCommand(ExecuteOpenSteuernCommand);
            ValidiereCommand = new DelegateCommand(ExecuteValidateCommand, CanExecuteSaveCommand);
            ValidateBetrag("");
            ValidateDatum(null, nameof(Zahldatum));
            ValidateDatum(null, nameof(Exdatum));
        }

        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.dividendeErhalten;

        public void Wertpapier(int wertpapierID, string wertpapierName)
        {
            Data.WertpapierID = wertpapierID;
            Title = "Dividende erhalten von " + wertpapierName;
            OnPropertyChanged(nameof(Title));
        }

        private void BetragGerundetBerechnen()
        {
            bool AllesKorrekt = true;
            if(!double.TryParse(Data.Betrag, out double betrag))
            {
                Werte.BetragGerundet = "";
                AllesKorrekt = false;
            }

            if (!double.TryParse(Data.Umrechnungskurs, out double Umrechnungskurs))
            {
                Werte.BetragGerundet = Data.Betrag;
                AllesKorrekt = false;
            }
            
            if (AllesKorrekt)
            {
                Werte.BetragGerundet = new DividendenBerechnungen().BetragUmgerechnet(betrag, Umrechnungskurs, true, Data.RundungArtDividende).ToString();
            }        
            OnPropertyChanged(nameof(Werte));
        }


        #region Bindings
        public ICommand OpenSteuernCommand { get; set; }
        public ICommand ValidiereCommand { get; set; }

        public DateTime? Exdatum
        {
            get => Data.Exdatum.Equals(DateTime.MinValue) ? null : Data.Exdatum;
            set
            {
                if (RequestIsWorking || !Equals(Data.Exdatum, value))
                {
                    ValidateDatum(value, nameof(Exdatum));
                    Data.Exdatum = value;
                    OnPropertyChanged();
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
                    (ValidiereCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public DateTime? Zahldatum
        {
            get => Data.Zahldatum.Equals(DateTime.MinValue) ? null : Data.Zahldatum;
            set
            {
                if (RequestIsWorking || !Equals(Data.Zahldatum, value))
                {
                    ValidateDatum(value, nameof(Zahldatum));
                    Data.Zahldatum = value;
                    OnPropertyChanged();
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
                    (ValidiereCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Betrag
        {
            get { return Data.Betrag; }
            set
            {
                if (RequestIsWorking || !string.Equals(Data.Betrag, value))
                {
                    if (!ValidateBetrag(value))
                    {
                        if (value == null || !value.Equals("0"))
                        {
                            Data.Betrag = "";
                            OnPropertyChanged();
                        }
                        return;
                    }
                    Data.Betrag = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                    (ValidiereCommand as DelegateCommand).RaiseCanExecuteChanged();
                    BetragGerundetBerechnen();
                }
            }
        }

        public string Wechselkurs
        {
            get => Data.Umrechnungskurs;
            set
            {
                if (RequestIsWorking || !string.Equals(Data.Umrechnungskurs, value))
                {
                    Data.Umrechnungskurs = value ?? "";
                    OnPropertyChanged(nameof(WechsellkursHasValue));
                    OnPropertyChanged();
                    BetragGerundetBerechnen();
                }
            }
        }

        public Waehrungen Waehrung
        {
            get => Data.Waehrung;
            set
            {
                if (RequestIsWorking || (Data.Waehrung != value))
                {
                    Data.Waehrung = value;
                    OnPropertyChanged();
                }
            }
        }
       
        public static IEnumerable<Waehrungen> Waehrungen => Enum.GetValues(typeof(Waehrungen)).Cast<Waehrungen>();

        public static IEnumerable<DividendenRundungTypes> RundungTypes => Enum.GetValues(typeof(DividendenRundungTypes)).Cast<DividendenRundungTypes>();

        public DividendenRundungTypes RundungArtDividende
        {
            get => Data.RundungArtDividende;
            set
            {
                if (RequestIsWorking || (Data.RundungArtDividende != value))
                {
                    Data.RundungArtDividende = value;
                    OnPropertyChanged();
                    BetragGerundetBerechnen();
                }
            }
        }

        public DividendenRundungTypes RundungArtErhalten
        {
            get => Data.RundungArtErhalten;
            set
            {
                if (RequestIsWorking || (Data.RundungArtErhalten != value))
                {
                    Data.RundungArtErhalten = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool WechsellkursHasValue => Data.Umrechnungskurs.Length > 0;

        public ErhalteneDividendeEintragenWerteModel Werte { get; private set; }

        #endregion

        #region Commands

        private void ExecuteOpenSteuernCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenSteuernUebersichtMessage(OpenSteuernUebersichtMessageCallback, Data.Steuer.Steuern), "ErhalteneDividendeEintragen");
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;

                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + $"/api/Depot/ErhalteneDividende", new ErhalteneDividendeEintragenDTO
                {
                    Betrag = double.Parse(Data.Betrag),
                    ExDatum = Data.Exdatum.Value,
                    RundungDividende = Data.RundungArtDividende,
                    RundungErhalten = Data.RundungArtErhalten,
                    WertpapierId = Data.WertpapierID,
                    ZahlDatum = Data.Zahldatum.Value,
                    Umrechnungskurs = double.TryParse(Data.Umrechnungskurs, out double Umrechnungskurs) ? (double?)Umrechnungskurs : null,
                    Waehrung = Data.Waehrung,
                    Steuer = Data.Steuer
                });

                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                     WeakReferenceMessenger.Default.Send(new CloseViewMessage { }, "ErhalteneDividendeEintragen");
                }
                else
                {
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                    return;
                }
            }
        }

        private async void ExecuteValidateCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + $"/api/Depot/ErhalteneDividendeWerte", new ErhalteneDividendeEintragenDTO
                {
                    Betrag = double.Parse(Data.Betrag),
                    ExDatum = Data.Exdatum.Value,
                    RundungDividende = Data.RundungArtDividende,
                    RundungErhalten = Data.RundungArtErhalten,
                    WertpapierId = Data.WertpapierID,
                    ZahlDatum = Data.Zahldatum.Value,
                    Umrechnungskurs = double.TryParse(Data.Umrechnungskurs, out double Umrechnungskurs) ? (double?)Umrechnungskurs : null,
                    Waehrung = Data.Waehrung,
                    Steuer = Data.Steuer
                });

                if (resp.IsSuccessStatusCode)
                {
                    ErhalteneDividendeEintragenWerteDTO WerteResponse = await resp.Content.ReadAsAsync<ErhalteneDividendeEintragenWerteDTO>();
                    Werte.Bemessungsgrundlage = WerteResponse.Bemessungsgrundlage.ToString();
                    Werte.Bestand = WerteResponse.Bestand.ToString();
                    Werte.Erhalten = WerteResponse.Erhalten.ToString();
                    Werte.SteuerNachZwischensumme= WerteResponse.SteuerNachZwischensumme.ToString();
                    Werte.SteuerVorZwischensumme = WerteResponse.SteuerVorZwischensumme.ToString();
                    Werte.Zwischensumme = WerteResponse.Zwischensumme.ToString();
                    OnPropertyChanged(nameof(Werte));
                }
                RequestIsWorking = false;
            }
        }

        #endregion

        #region Callbacks

        private void OpenSteuernUebersichtMessageCallback(bool confirmed, IList<SteuerModel> steuern)
        {
            if (confirmed)
            {
                Data.Steuer.Steuern = steuern;
            }
        }
        #endregion

        #region Validate      

        private bool ValidateDatum(DateTime? datun, string fieldname)
        {
            BaseValidierung Validierung = new();

            bool isValid = Validierung.ValidateDatum(datun, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }

        private bool ValidateBetrag(string betrag)
        {
            BaseValidierung Validierung = new();

            bool isValid = Validierung.ValidateBetrag(betrag, out ICollection<string> validationErrors, true);

            AddValidateInfo(isValid, "Betrag", validationErrors);
            return isValid;
        }

        #endregion

        protected override void OnActivated()
        {
            Data = new ErhalteneDividendeEintragenModel { Steuer = new SteuergruppeModel { Steuern = [] } };
            Werte = new();
            state = State.Neu;
        }
    }
}