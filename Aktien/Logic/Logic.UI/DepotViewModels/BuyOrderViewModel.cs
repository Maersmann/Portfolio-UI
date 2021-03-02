using Aktien.Data.Types.WertpapierTypes;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Logic.Core.DepotLogic.Exceptions;
using Aktien.Data.Types;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;
using Aktien.Logic.Core.DepotLogic.Classes;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class BuyOrderViewModel : ViewModelStammdaten
    {
        private OrderHistory data;
        private BuySell buySell;
        private WertpapierTypes typ;
        private Double? betrag;
        private Double gesamtbetrag;
        private Double preisUebersicht;
        private Double buyIn;
        public BuyOrderViewModel()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Cleanup();
        }

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
                buyIn = new KaufBerechnungen().BuyInAktieGekauft(0, 0, data.Anzahl, preisUebersicht, data.Anzahl, data.Fremdkostenzuschlag);
            else
                buyIn = 0;

            gesamtbetrag = Math.Round(preisUebersicht * data.Anzahl, 3, MidpointRounding.AwayFromZero) + data.Fremdkostenzuschlag.GetValueOrDefault(0);

            this.RaisePropertyChanged("Gesamtbetrag");
            this.RaisePropertyChanged("PreisBerechnet");
            this.RaisePropertyChanged("BuyIn");
        }

        protected override void ExecuteSaveCommand()
        {
            var Depot = new DepotAPI();
            if (buySell.Equals(BuySell.Buy))
            {
                try
                {
                    Depot.WertpapierGekauft(data.Preis, data.Fremdkostenzuschlag, data.Orderdatum, WertpapierID, data.Anzahl, data.KaufartTyp, data.OrderartTyp, betrag);
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewAusgabenUebersicht);
                }
                catch (NeuereOrderVorhandenException)
                {
                    SendExceptionMessage("Es sind neuere Orders vorhanden.");
                    return;
                }                        
            }
            else
            {
                try
                {
                    Depot.WertpapierVerkauft(data.Preis, data.Fremdkostenzuschlag, data.Orderdatum, WertpapierID, data.Anzahl, data.KaufartTyp, data.OrderartTyp, betrag);
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewEinnahmenUebersicht);
                }
                catch (ZuVieleWertpapiereVerkaufException)
                {
                    SendExceptionMessage("Es wurden mehr Wertpapiere zum Verkauf eingetragen, als im Depot vorhanden.");                   
                    return;
                }
                catch (NeuereOrderVorhandenException)
                {
                    SendExceptionMessage("Es sind neuere Orders vorhanden.");
                    return;
                }

            }
            Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Buy-Order gespeichert." }, "BuyOrder");
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewDepotUebersicht);
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewOrderUebersicht);
        }

        public void SetTitle(BuySell buySell, WertpapierTypes types)
        {
            this.buySell = buySell;
            typ = types;
            this.RaisePropertyChanged("KauftypBez");
            this.RaisePropertyChanged("Titel");
            this.RaisePropertyChanged("BuySell");
        }


        #region Bindings

        public IEnumerable<KaufTypes> KaufTypes
        {
            get
            {
                return Enum.GetValues(typeof(KaufTypes)).Cast<KaufTypes>();
            }
        }
        public IEnumerable<OrderTypes> OrderTypes
        {
            get
            {
                return Enum.GetValues(typeof(OrderTypes)).Cast<OrderTypes>();
            }
        }
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
                    else if  (data.OrderartTyp.Equals(Data.Types.WertpapierTypes.OrderTypes.Sparplan))
                    {
                        Betrag = null;
                        Preis = null;
                        DeleteValidateInfo("Betrag");
                    }
                    LoadAktie = false;
                    this.data.OrderartTyp = value;
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
        public int WertpapierID { get; set; }
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


        public double Gesamtbetrag { get { return gesamtbetrag; } }
        public double PreisBerechnet { get { return preisUebersicht; } }
        public double BuyIn { get { return buyIn; } }

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

        public override void Cleanup()
        {
            state = State.Neu;
            data = new OrderHistory();
            KaufTyp = Data.Types.WertpapierTypes.KaufTypes.Kauf;
            OrderTyp = Data.Types.WertpapierTypes.OrderTypes.Normal;
            Anzahl = null;
            Preis = null;
            Datum = DateTime.Now;
            Fremdkosten = null;
            Betrag = null;
            preisUebersicht = 0;
            gesamtbetrag = 0;
            buyIn = 0;
        }
    }
}
