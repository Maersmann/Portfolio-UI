using Aktien.Data.Types.WertpapierTypes;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
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

namespace Aktien.Logic.UI.DepotViewModels
{
    public class BuyOrderViewModel : ViewModelStammdaten<BuyOrderModel>
    {
        private BuySell buySell;
        private WertpapierTypes typ;
        private Double? betrag;
        private Double preisUebersicht;
        private Double buyIn;
        private bool neueOrderNichtGespeichert;
        private double steuern;

        public BuyOrderViewModel()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            OpenSteuernCommand = new DelegateCommand(this.ExecuteOpenOpenSteuernCommand, this.CanExecuteOpenOpenSteuernCommand);
            Cleanup();
        }

        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.buysell;

        public void BerechneWerte()
        {
            if (betrag.HasValue)
            {
                if (betrag.Value.Equals(0))
                    preisUebersicht = 0;
                else
                    preisUebersicht = Math.Round(betrag.Value / data.Anzahl, 3, MidpointRounding.AwayFromZero);
            }              
            else
                preisUebersicht = Preis.GetValueOrDefault(0);

            if ((buySell.Equals(BuySell.Buy)) && (preisUebersicht != 0))
                buyIn = new KaufBerechnungen().BuyInAktieGekauft(0, 0, data.Anzahl, preisUebersicht, data.Anzahl, data.Fremdkostenzuschlag, data.OrderartTyp);
            else
                buyIn = 0;

            data.Bemessungsgrundlage = Math.Round(preisUebersicht * data.Anzahl, 3, MidpointRounding.AwayFromZero);
            steuern = new SteuerBerechnen().SteuerGesamt(data.Steuer.Steuern);
            if(BuySell.Equals(BuySell.Buy))
                data.Gesamt = data.Bemessungsgrundlage + data.Fremdkostenzuschlag.GetValueOrDefault(0);
            else
                data.Gesamt = data.Bemessungsgrundlage - data.Fremdkostenzuschlag.GetValueOrDefault(0) + steuern;

            this.RaisePropertyChanged(nameof(Gesamt));
            this.RaisePropertyChanged(nameof(Bemessungsgrundlage));
            this.RaisePropertyChanged("PreisBerechnet");
            this.RaisePropertyChanged("BuyIn");
            this.RaisePropertyChanged(nameof(Steuern));
        }
        public void SetTitle(BuySell buySell, WertpapierTypes types)
        {
            this.buySell = buySell;
            typ = types;
            ((DelegateCommand)OpenSteuernCommand).RaiseCanExecuteChanged();
            this.RaisePropertyChanged("KauftypBez");
            this.RaisePropertyChanged("Titel");
            this.RaisePropertyChanged("BuySell");
            this.RaisePropertyChanged("KaufTypes");
            this.RaisePropertyChanged("OrderTypes");
        }

        #region Commands
        private void ExecuteOpenOpenSteuernCommand()
        {
            Messenger.Default.Send<OpenSteuernUebersichtMessage>(new OpenSteuernUebersichtMessage(OpenSteuernUebersichtMessageCallback, data.SteuergruppeID, !neueOrderNichtGespeichert), "BuyOrder");
        }
        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+ $"/api/Depot/Order/new?buysell={buySell}", data);

                if (resp.IsSuccessStatusCode)
                {
                    neueOrderNichtGespeichert = false;
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Buy-Order gespeichert." }, GetStammdatenTyp());
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), GetStammdatenTyp());              
                }
                else
                {
                    if ((int)resp.StatusCode == 901)
                        SendExceptionMessage("Es sind neuere Orders vorhanden.");
                    else if ((int)resp.StatusCode == 902)
                        SendExceptionMessage("Es wurden mehr Wertpapiere zum Verkauf eingetragen, als im Depot vorhanden.");
                    else
                        SendExceptionMessage("Order konnte nicht gespeichert werden.");

                    return;
                }
            }
        }

        protected override async void ExecuteCloseCommand()
        {
            if (data.SteuergruppeID.HasValue && state.Equals(State.Neu) && neueOrderNichtGespeichert)
            {
                if (GlobalVariables.ServerIsOnline)
                {
                    HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/dividendeErhalten/Steuern/{data.SteuergruppeID}");
                    if (!resp.IsSuccessStatusCode)
                    {
                        SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                        return;
                    }

                }
            }
            base.ExecuteCloseCommand();
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
            get { return data.KaufartTyp; }
            set
            {
                if (LoadAktie || (this.data.KaufartTyp != value))
                {
                    ValidateBetrag(Preis, value,"Preis");
                    this.data.KaufartTyp = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public OrderTypes OrderTyp
        {
            get { return data.OrderartTyp; }
            set
            {
                if (LoadAktie || (this.data.OrderartTyp != value))
                {
                    LoadAktie = true;
                    if (value.Equals(Data.Types.WertpapierTypes.OrderTypes.Sparplan))
                    {
                        Preis = null;
                        Betrag = null;
                        DeleteValidateInfo("Preis");
                    }
                    else if (data.OrderartTyp.Equals(Data.Types.WertpapierTypes.OrderTypes.Sparplan))
                    {
                        Betrag = null;
                        Preis = null;
                        DeleteValidateInfo("Betrag");
                    }
                    LoadAktie = false;
                    this.data.OrderartTyp = value;
                    BerechneWerte();
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged("EingabePreisEnabled");
                    this.RaisePropertyChanged("EingabeGesamtbetragEnabled");
                    this.RaisePropertyChanged("isOrderTypSparplan");
                }
            }
        }
        public double? Anzahl
        {
            get
            {
                return data.Anzahl;
            }
            set
            {
                if (LoadAktie || (this.data.Anzahl != value))
                {
                    ValidateAnzahl(value);
                    this.data.Anzahl = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                    BerechneWerte();
                }
            }
        }
        public double? Fremdkosten
        {
            get
            {
                return data.Fremdkostenzuschlag;
            }
            set
            {
                if (LoadAktie || (this.data.Fremdkostenzuschlag != value))
                {
                    this.data.Fremdkostenzuschlag = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(FremdkostenNegativ));
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                    BerechneWerte();
                }
            }
        }
        public double? Preis
        {
            get
            {
                return data.Preis;
            }
            set
            {
                if (LoadAktie || (this.data.Preis != value))
                {
                    ValidateBetrag(value, KaufTyp, "Preis");
                    this.data.Preis = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                    BerechneWerte();
                }
            }
        }
        public DateTime? Datum
        {
            get
            {
                return data.Orderdatum;
            }
            set
            {
                if (LoadAktie || (!DateTime.Equals(this.data.Orderdatum, value)))
                {
                    ValidateDatum(value);
                    this.data.Orderdatum = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public String KauftypBez 
        { 
            get 
            {
                if (buySell == BuySell.Buy)
                {
                    return "Kauf Art";
                }
                else
                {
                    return "Verkauf Art";
                }
            } 
        }
        public String Titel
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
        public Double? Betrag
        {
            get
            {
                return betrag;
            }
            set
            {
                if (LoadAktie || (this.betrag != value))
                {
                    ValidateBetrag(value, KaufTyp, "Betrag");
                    this.betrag = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
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
                if (!Fremdkosten.HasValue) return 0;
                else return (Fremdkosten.GetValueOrDefault(0) * -1);
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
            return isValid;
        }

        private bool ValidateBetrag(Double? betrag, KaufTypes kaufTyp, string propertyKey)
        {
            var Validierung = new AktieGekauftValidierung();

            bool isValid = Validierung.ValidateBetrag(betrag, kaufTyp, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, propertyKey, validationErrors);
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
        private async void OpenSteuernUebersichtMessageCallback(bool confirmed, int id)
        {
            if (confirmed)
            {
                data.SteuergruppeID = id;
                if (GlobalVariables.ServerIsOnline)
                {
                    HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/Steuern?steuergruppeid={id}");
                    if (resp.IsSuccessStatusCode)
                        data.Steuer.Steuern = await resp.Content.ReadAsAsync<ObservableCollection<SteuerModel>>();
                    else
                        SendExceptionMessage(await resp.Content.ReadAsStringAsync());
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
            Anzahl = null;
            Preis = null;
            Datum = DateTime.Now;
            Fremdkosten = null;
            Betrag = null;
            preisUebersicht = 0;
            data.Gesamt = 0;
            data.Bemessungsgrundlage = 0;
            buyIn = 0;
            steuern = 0;
        }
    }
}
