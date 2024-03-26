using Aktien.Data.Types.WertpapierTypes;
using CommunityToolkit.Mvvm.Messaging;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Messages.Base;
using Base.Logic.ViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aktien.Data.Types;
using Aktien.Logic.Core.DepotLogic;
using Data.Model.DepotModels;
using Aktien.Logic.Core;
using System.Net.Http;
using System.Net;
using System.Collections.ObjectModel;
using Data.Model.SteuerModels;
using System.Windows.Input;

using Logic.Messages.SteuernMessages;
using Logic.Core.SteuernLogic;
using Logic.UI.DepotViewModels.Helper;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;
using Base.Logic.Wrapper;
using Data.Types.SteuerTypes;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class BuyOrderViewModel : ViewModelStammdaten<BuyOrderModel, StammdatenTypes>
    {
        private BuySell buySell;
        private WertpapierTypes typ;    
        private double preisUebersicht;
        private double buyIn;
        private double steuern;
        private string anzahl;
        private string preis;
        private string betrag;
        private string fremdkosten;

        public BuyOrderViewModel()
        {
            SaveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            OpenSteuernCommand = new DelegateCommand(ExecuteOpenOpenSteuernCommand, CanExecuteOpenOpenSteuernCommand);
        }

        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.buysell;

        public void BerechneWerte()
        {
            if ((betrag.Length > 0) && (double.TryParse(betrag, out double Betrag)))
            {
                if (Betrag.Equals(0))
                {
                    preisUebersicht = 0;
                }
                else
                {
                    preisUebersicht = Data.Anzahl.Equals(0) ? 0 : Math.Round(Betrag / Data.Anzahl, 3, MidpointRounding.AwayFromZero);
                }
            }              
            else
            {
                preisUebersicht = Data.Preis;
            }

            buyIn = buySell.Equals(BuySell.Buy) && (preisUebersicht != 0) && (Data.Anzahl != 0)
                ? new KaufBerechnungen().BuyInAktieGekauft(0, 0, Data.Anzahl, preisUebersicht, Data.Anzahl, Data.Fremdkostenzuschlag, Data.OrderartTyp)
                : 0;

            Data.Bemessungsgrundlage = Math.Round(preisUebersicht * Data.Anzahl, 3, MidpointRounding.AwayFromZero);
            steuern = new SteuerBerechnen().SteuerGesamt(Data.Steuer.Steuern);
            Data.Gesamt = BuySell.Equals(BuySell.Buy)
                ? Data.Bemessungsgrundlage + Data.Fremdkostenzuschlag.GetValueOrDefault(0)
                : Data.Bemessungsgrundlage - Data.Fremdkostenzuschlag.GetValueOrDefault(0) + steuern;

            OnPropertyChanged(nameof(Gesamt));
            OnPropertyChanged(nameof(Bemessungsgrundlage));
            OnPropertyChanged(nameof(PreisBerechnet));
            OnPropertyChanged(nameof(BuyIn));
            OnPropertyChanged(nameof(Steuern));
        }
        public void SetTitle(BuySell buySell, WertpapierTypes types)
        {
            this.buySell = buySell;
            typ = types;
            ((DelegateCommand)OpenSteuernCommand).RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(KauftypBez));
            OnPropertyChanged(nameof(Titel));
            OnPropertyChanged(nameof(BuySell));
            OnPropertyChanged(nameof(KaufTypes));
            OnPropertyChanged(nameof(OrderTypes));
        }

        #region Commands
        private void ExecuteOpenOpenSteuernCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenSteuernUebersichtMessage(OpenSteuernUebersichtMessageCallback), "BuyOrder");
        }
        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+ $"/api/Depot/Order/new?buysell={buySell}", Data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                     WeakReferenceMessenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Buy-Order gespeichert." }, GetStammdatenTyp().ToString());
                     WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());              
                }
                else
                {
                    if ((int)resp.StatusCode == 901)
                    {
                        SendExceptionMessage("Es sind neuere Orders vorhanden.");
                    }
                    else if ((int)resp.StatusCode == 902)
                    {
                        SendExceptionMessage("Es wurden mehr Wertpapiere zum Verkauf eingetragen, als im Depot vorhanden.");
                    }
                    else
                    {
                        SendExceptionMessage("Order konnte nicht gespeichert werden.");
                    }

                    return;
                }
            }
        }

        private bool CanExecuteOpenOpenSteuernCommand()
        {
            return buySell.Equals(BuySell.Sell);
        }
        #endregion

        #region Bindings

        public IEnumerable<KaufTypes> KaufTypes => BuyOrderHelper.GetKaufTypes(buySell);
        public IEnumerable<OrderTypes> OrderTypes => BuyOrderHelper.GetOrderTypes(buySell);

        public KaufTypes KaufTyp
        {
            get => Data.KaufartTyp;
            set
            {
                if (RequestIsWorking || (Data.KaufartTyp != value))
                {
                    ValidateBetrag(Data.Preis, value, "Preis");
                    Data.KaufartTyp = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public OrderTypes OrderTyp
        {
            get { return Data.OrderartTyp; }
            set
            {
                if (RequestIsWorking || (Data.OrderartTyp != value))
                {
                    if (value.Equals(Aktien.Data.Types.WertpapierTypes.OrderTypes.Sparplan))
                    {
                        Preis = "";
                        Betrag = "";
                        DeleteValidateInfo("Preis");
                    }
                    else if (Data.OrderartTyp.Equals(Aktien.Data.Types.WertpapierTypes.OrderTypes.Sparplan))
                    {
                        Betrag = "";
                        Preis = "";
                        DeleteValidateInfo("Betrag");
                    }
                    Data.OrderartTyp = value;
                    BerechneWerte();
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(EingabePreisEnabled));
                    OnPropertyChanged(nameof(EingabeGesamtbetragEnabled));
                    OnPropertyChanged(nameof(isOrderTypSparplan));
                }
            }
        }
        public string Anzahl
        {
            get => anzahl;
            set
            {
                if (!double.TryParse(value, out double Anzahl))
                {
                    ValidateAnzahl(0);
                    anzahl = "";
                    Data.Anzahl = 0;
                    OnPropertyChanged();
                    return;
                }
                anzahl = value;
                if (RequestIsWorking || (Data.Anzahl != Anzahl))
                {
                    ValidateAnzahl(Anzahl);
                    Data.Anzahl = Anzahl;
                    OnPropertyChanged();
                    BerechneWerte();
                }
            }
        }
        public string Fremdkosten
        {
            get => fremdkosten;
            set
            {
                if (!double.TryParse(value, out double Fremdkosten))
                {
                    fremdkosten = "";
                    Data.Fremdkostenzuschlag = 0;
                    OnPropertyChanged();
                    return;
                }
                fremdkosten = value;
                if (RequestIsWorking || (Data.Fremdkostenzuschlag != Fremdkosten))
                {
                    Data.Fremdkostenzuschlag = Fremdkosten;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FremdkostenNegativ));
                    BerechneWerte();
                }
            }
        }
        public string Preis
        {
            get => preis;
            set
            {
                if (!double.TryParse(value, out double Preis))
                {
                    ValidateBetrag(0, KaufTyp, "Preis");
                    preis = "";
                    Data.Preis = 0;
                    OnPropertyChanged();
                    return; 
                }
                preis = value;
                if (RequestIsWorking || (Data.Preis != Preis))
                {
                    ValidateBetrag(Preis, KaufTyp, "Preis");
                    Data.Preis = Preis;
                    OnPropertyChanged();                  
                    BerechneWerte();
                }
            }
        }
        public DateTime? Datum
        {
            get => Data.Orderdatum;
            set
            {
                if (RequestIsWorking || (!DateTime.Equals(this.Data.Orderdatum, value)))
                {
                    ValidateDatum(value);
                    Data.Orderdatum = value.GetValueOrDefault();
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string KauftypBez => buySell == BuySell.Buy ? "Kauf Art" : "Verkauf Art";
        public string Titel
        {
            get
            {
                var Wertpapiertyp = "";

                switch (typ)        
                {
                    case WertpapierTypes.Aktie: Wertpapiertyp = "Aktie"; break;
                    case WertpapierTypes.ETF: Wertpapiertyp = "ETF"; break;
                    case WertpapierTypes.Derivate: Wertpapiertyp = "Derivate"; break;
                }

                if (buySell == BuySell.Buy)
                {
                    return  Wertpapiertyp + " gekauft";
                }
                else
                {
                    return Wertpapiertyp + " verkauft";
                }
            }
        }
        public int WertpapierID { set { Data.WertpapierID = value; } }
        public string Betrag
        {
            get => betrag;
            set
            {
                if (!double.TryParse(value, out double Betrag))
                {
                    ValidateBetrag(0, KaufTyp, "Betrag");
                    betrag = "";
                    OnPropertyChanged();
                    return; 
                }
                
                if (RequestIsWorking || (betrag != value))
                {
                    ValidateBetrag(Betrag, KaufTyp, "Betrag");
                    betrag = value;
                    OnPropertyChanged();   
                    BerechneWerte();
                }
            }
        }

        public bool EingabePreisEnabled { get { return Data.OrderartTyp != Aktien.Data.Types.WertpapierTypes.OrderTypes.Sparplan; } }
        public bool EingabeGesamtbetragEnabled { get { return Data.OrderartTyp == Aktien.Data.Types.WertpapierTypes.OrderTypes.Sparplan; } }
        public bool isOrderTypSparplan { get { return Data.OrderartTyp != Aktien.Data.Types.WertpapierTypes.OrderTypes.Sparplan; } }
        public BuySell BuySell { get { return buySell; } }


        public double Gesamt { get { return Data.Gesamt; } }
        public double Bemessungsgrundlage { get { return Data.Bemessungsgrundlage; } }
        public double PreisBerechnet { get { return preisUebersicht; } }
        public double BuyIn { get { return buyIn; } }
        public double Steuern { get { return steuern; } }
        public double FremdkostenNegativ 
        { 
            get 
            {
                if (Fremdkosten.Equals("")) return 0;
                else return (Double.Parse(Fremdkosten) * -1);
            } 
        } 

        public ICommand OpenSteuernCommand { get; set; }

        #endregion

        #region Validierung

        private bool ValidateAnzahl(double? anzahl)
        {
            var Validierung = new AktieGekauftValidierung();

            bool isValid = Validierung.ValidateAnzahl(anzahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Anzahl", validationErrors);
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            return isValid;
        }

        private bool ValidateBetrag(Double? betrag, KaufTypes kaufTyp, string propertyKey)
        {
            var Validierung = new AktieGekauftValidierung();

            bool isValid = Validierung.ValidateBetrag(betrag, kaufTyp, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, propertyKey, validationErrors);
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            return isValid;
        }

        private bool ValidateDatum(DateTime? datum)
        {
            var Validierung = new AktieGekauftValidierung();

            bool isValid = Validierung.ValidateDatum(datum, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Datum", validationErrors);
            return isValid;
        }

        #endregion

        #region Callbacks
        private void OpenSteuernUebersichtMessageCallback(bool confirmed, IList<SteuerModel> steuern)
        {
            if (confirmed)
            {
                Data.Steuer.Steuern = steuern;
                BerechneWerte();
            }
        }
        #endregion

        protected override void OnActivated()
        {
            DeleteValidateInfo("Betrag");
            state = State.Neu;
            Data = new BuyOrderModel { Steuer = new SteuergruppeModel { SteuerHerkunftTyp = SteuerHerkunftTyp.shtOrder, Steuern = [] } };
            KaufTyp = Aktien.Data.Types.WertpapierTypes.KaufTypes.Kauf;
            OrderTyp = Aktien.Data.Types.WertpapierTypes.OrderTypes.Normal;
            Preis = "";
            Datum = DateTime.Now;
            Fremdkosten = "";
            Betrag = null;
            preisUebersicht = 0;
            Data.Gesamt = 0;
            Data.Bemessungsgrundlage = 0;
            buyIn = 0;
            steuern = 0;
            Anzahl = "";
            Betrag = "";
            DeleteValidateInfo("Betrag");
        }
    }
}
