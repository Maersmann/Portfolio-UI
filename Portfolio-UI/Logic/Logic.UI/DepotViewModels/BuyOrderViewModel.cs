using Aktien.Data.Types.WertpapierTypes;
using GalaSoft.MvvmLight.Messaging;
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
using GalaSoft.MvvmLight.Command;
using Logic.Messages.SteuernMessages;
using Logic.Core.SteuernLogic;
using Logic.UI.DepotViewModels.Helper;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class BuyOrderViewModel : ViewModelStammdaten<BuyOrderModel, StammdatenTypes>
    {
        private BuySell buySell;
        private WertpapierTypes typ;    
        private Double preisUebersicht;
        private Double buyIn;
        private bool neueOrderNichtGespeichert;
        private double steuern;
        private string anzahl;
        private string preis;
        private string betrag;
        private string fremdkosten;

        public BuyOrderViewModel()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            OpenSteuernCommand = new DelegateCommand(this.ExecuteOpenOpenSteuernCommand, this.CanExecuteOpenOpenSteuernCommand);
            Cleanup();
        }

        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.buysell;

        public void BerechneWerte()
        {
            if ((betrag.Length > 0) && (double.TryParse(betrag, out double Betrag)))
            {
                if (Betrag.Equals(0))
                    preisUebersicht = 0;
                else if (data.Anzahl.Equals(0))
                    preisUebersicht = 0;
                else
                    preisUebersicht = Math.Round(Betrag / data.Anzahl, 3, MidpointRounding.AwayFromZero);
            }              
            else
                preisUebersicht = data.Preis;

            if ((buySell.Equals(BuySell.Buy)) && (preisUebersicht != 0) && (data.Anzahl != 0))
                buyIn = new KaufBerechnungen().BuyInAktieGekauft(0, 0, data.Anzahl, preisUebersicht, data.Anzahl, data.Fremdkostenzuschlag, data.OrderartTyp);
            else
                buyIn = 0;

            data.Bemessungsgrundlage = Math.Round(preisUebersicht * data.Anzahl, 3, MidpointRounding.AwayFromZero);
            steuern = new SteuerBerechnen().SteuerGesamt(data.Steuer.Steuern);
            if(BuySell.Equals(BuySell.Buy))
                data.Gesamt = data.Bemessungsgrundlage + data.Fremdkostenzuschlag.GetValueOrDefault(0);
            else
                data.Gesamt = data.Bemessungsgrundlage - data.Fremdkostenzuschlag.GetValueOrDefault(0) + steuern;

            RaisePropertyChanged(nameof(Gesamt));
            RaisePropertyChanged(nameof(Bemessungsgrundlage));
            RaisePropertyChanged("PreisBerechnet");
            RaisePropertyChanged("BuyIn");
            RaisePropertyChanged(nameof(Steuern));
        }
        public void SetTitle(BuySell buySell, WertpapierTypes types)
        {
            this.buySell = buySell;
            typ = types;
            ((DelegateCommand)OpenSteuernCommand).RaiseCanExecuteChanged();
            RaisePropertyChanged("KauftypBez");
            RaisePropertyChanged("Titel");
            RaisePropertyChanged("BuySell");
            RaisePropertyChanged("KaufTypes");
            RaisePropertyChanged("OrderTypes");
        }

        #region Commands
        private void ExecuteOpenOpenSteuernCommand()
        {
            Messenger.Default.Send(new OpenSteuernUebersichtMessage(OpenSteuernUebersichtMessageCallback, data.SteuergruppeID, !neueOrderNichtGespeichert), "BuyOrder");
        }
        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+ $"/api/Depot/Order/new?buysell={buySell}", data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    neueOrderNichtGespeichert = false;
                    Messenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Buy-Order gespeichert." }, GetStammdatenTyp());
                    Messenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());              
                }
                else
                {
                    if ((int)resp.StatusCode == 901)
                    {
                        SendExceptionMessage("Es sind neuere Orders vorhanden.");
                    }
                    else if ((int)resp.StatusCode == 902)
                        SendExceptionMessage("Es wurden mehr Wertpapiere zum Verkauf eingetragen, als im Depot vorhanden.");
                    else
                    {
                        SendExceptionMessage("Order konnte nicht gespeichert werden.");
                    }

                    return;
                }
            }
        }

        protected override async void ExecuteCleanUpCommand()
        {
            if (data.SteuergruppeID.HasValue && state.Equals(State.Neu) && neueOrderNichtGespeichert)
            {
                if (GlobalVariables.ServerIsOnline)
                {
                    RequestIsWorking = true;
                    HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/dividendeErhalten/Steuern/{data.SteuergruppeID}");
                    RequestIsWorking = false;
                    if (!resp.IsSuccessStatusCode)
                    {
                        SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                        return;
                    }

                }
            }
            base.ExecuteCleanUpCommand();
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
            get => data.KaufartTyp;
            set
            {
                if (RequestIsWorking || (data.KaufartTyp != value))
                {
                    ValidateBetrag(data.Preis, value, "Preis");
                    data.KaufartTyp = value;
                    RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public OrderTypes OrderTyp
        {
            get { return data.OrderartTyp; }
            set
            {
                if (RequestIsWorking || (data.OrderartTyp != value))
                {
                    if (value.Equals(Data.Types.WertpapierTypes.OrderTypes.Sparplan))
                    {
                        Preis = "";
                        Betrag = "";
                        DeleteValidateInfo("Preis");
                    }
                    else if (data.OrderartTyp.Equals(Data.Types.WertpapierTypes.OrderTypes.Sparplan))
                    {
                        Betrag = "";
                        Preis = "";
                        DeleteValidateInfo("Betrag");
                    }
                    data.OrderartTyp = value;
                    BerechneWerte();
                    RaisePropertyChanged();
                    RaisePropertyChanged("EingabePreisEnabled");
                    RaisePropertyChanged("EingabeGesamtbetragEnabled");
                    RaisePropertyChanged("isOrderTypSparplan");
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
                    data.Anzahl = 0;
                    RaisePropertyChanged();
                    return;
                }
                anzahl = value;
                if (RequestIsWorking || (data.Anzahl != Anzahl))
                {
                    ValidateAnzahl(Anzahl);
                    data.Anzahl = Anzahl;
                    RaisePropertyChanged();
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
                    data.Fremdkostenzuschlag = 0;
                    RaisePropertyChanged();
                    return;
                }
                fremdkosten = value;
                if (RequestIsWorking || (data.Fremdkostenzuschlag != Fremdkosten))
                {
                    data.Fremdkostenzuschlag = Fremdkosten;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(FremdkostenNegativ));
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
                    data.Preis = 0;
                    RaisePropertyChanged();
                    return; 
                }
                preis = value;
                if (RequestIsWorking || (data.Preis != Preis))
                {
                    ValidateBetrag(Preis, KaufTyp, "Preis");
                    data.Preis = Preis;
                    RaisePropertyChanged();                  
                    BerechneWerte();
                }
            }
        }
        public DateTime? Datum
        {
            get => data.Orderdatum;
            set
            {
                if (RequestIsWorking || (!DateTime.Equals(this.data.Orderdatum, value)))
                {
                    ValidateDatum(value);
                    data.Orderdatum = value.GetValueOrDefault();
                    RaisePropertyChanged();
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
        public int WertpapierID { set { data.WertpapierID = value; } }
        public string Betrag
        {
            get => betrag;
            set
            {
                if (!double.TryParse(value, out double Betrag))
                {
                    ValidateBetrag(0, KaufTyp, "Betrag");
                    betrag = "";
                    RaisePropertyChanged();
                    return; 
                }
                
                if (RequestIsWorking || (betrag != value))
                {
                    ValidateBetrag(Betrag, KaufTyp, "Betrag");
                    betrag = value;
                    RaisePropertyChanged();   
                    BerechneWerte();
                }
            }
        }

        public bool EingabePreisEnabled { get { return data.OrderartTyp != Data.Types.WertpapierTypes.OrderTypes.Sparplan; } }
        public bool EingabeGesamtbetragEnabled { get { return data.OrderartTyp == Data.Types.WertpapierTypes.OrderTypes.Sparplan; } }
        public bool isOrderTypSparplan { get { return data.OrderartTyp != Data.Types.WertpapierTypes.OrderTypes.Sparplan; } }
        public BuySell BuySell { get { return buySell; } }


        public double Gesamt { get { return data.Gesamt; } }
        public double Bemessungsgrundlage { get { return data.Bemessungsgrundlage; } }
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
        private async void OpenSteuernUebersichtMessageCallback(bool confirmed, int? id)
        {
            if (confirmed)
            {
                data.SteuergruppeID = id;
                if (GlobalVariables.ServerIsOnline)
                {
                    RequestIsWorking = true;
                    HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/Steuern?steuergruppeid={id}");
                    if (resp.IsSuccessStatusCode)
                        data.Steuer.Steuern = await resp.Content.ReadAsAsync<ObservableCollection<SteuerModel>>();
                    else
                        SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                    RequestIsWorking = false;
                }
                BerechneWerte();
            }
        }
        #endregion

        public override void Cleanup()
        {
            DeleteValidateInfo("Betrag");
            neueOrderNichtGespeichert = true;
            state = State.Neu;
            data = new BuyOrderModel { Steuer = new SteuergruppeModel { Steuern = new List<SteuerModel>() } };
            KaufTyp = Data.Types.WertpapierTypes.KaufTypes.Kauf;
            OrderTyp = Data.Types.WertpapierTypes.OrderTypes.Normal;
            Preis = "";
            Datum = DateTime.Now;
            Fremdkosten = "";
            Betrag = null;
            preisUebersicht = 0;
            data.Gesamt = 0;
            data.Bemessungsgrundlage = 0;
            buyIn = 0;
            steuern = 0;
            Anzahl = "";
            Betrag = "";
            DeleteValidateInfo("Betrag");
        }
    }
}
