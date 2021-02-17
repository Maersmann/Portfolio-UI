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

namespace Aktien.Logic.UI.DepotViewModels
{
    public class BuyOrderViewModel : ViewModelStammdaten
    {
        private OrderHistory data;
        private BuySell buySell;
        private WertpapierTypes typ;
        private Double? gesamtbetrag;
        public BuyOrderViewModel()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);

            Cleanup();
        }

        protected override void ExecuteSaveCommand()
        {
            var Depot = new DepotAPI();
            if (buySell.Equals(BuySell.Buy))
            {
                try
                {
                    Depot.WertpapierGekauft(data.Preis, data.Fremdkostenzuschlag, data.Orderdatum, WertpapierID, data.Anzahl, data.KaufartTyp, data.OrderartTyp, gesamtbetrag);
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
                    Depot.WertpapierVerkauft(data.Preis, data.Fremdkostenzuschlag, data.Orderdatum, WertpapierID, data.Anzahl, data.KaufartTyp, data.OrderartTyp, gesamtbetrag);
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

        public void SetTitle(BuySell inBuySell, WertpapierTypes inTypes)
        {
            buySell = inBuySell;
            typ = inTypes;
            this.RaisePropertyChanged("KauftypBez");
            this.RaisePropertyChanged("Titel");
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
                        Gesamtbetrag = null;
                        DeleteValidateInfo("Preis");
                    }
                    else if  (data.OrderartTyp.Equals(Data.Types.WertpapierTypes.OrderTypes.Sparplan))
                    {
                        Gesamtbetrag = null;
                        Preis = null;
                        DeleteValidateInfo("Gesamtbetrag");
                    }
                    LoadAktie = false;
                    this.data.OrderartTyp = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged("EingabePreisEnabled");
                    this.RaisePropertyChanged("EingabeGesamtbetragEnabled");
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

        public Double? Gesamtbetrag
        {
            get
            {
                return gesamtbetrag;
            }
            set
            {
                if (LoadAktie || (this.gesamtbetrag != value))
                {
                    ValidateBetrag(value, KaufTyp, "Gesamtbetrag");
                    this.gesamtbetrag = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public bool EingabePreisEnabled { get { return data.OrderartTyp != Data.Types.WertpapierTypes.OrderTypes.Sparplan; } }
        public bool EingabeGesamtbetragEnabled { get { return data.OrderartTyp == Data.Types.WertpapierTypes.OrderTypes.Sparplan; } }

        #endregion

        #region Validierung

        private bool ValidateAnzahl(double? inAnzahl)
        {
            var Validierung = new AktieGekauftValidierung();

            bool isValid = Validierung.ValidateAnzahl(inAnzahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Anzahl", validationErrors);
            return isValid;
        }

        private bool ValidateBetrag(Double? inBetrag, KaufTypes inKaufTyp, string inPropertyKey)
        {
            var Validierung = new AktieGekauftValidierung();

            bool isValid = Validierung.ValidateBetrag(inBetrag, inKaufTyp, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, inPropertyKey, validationErrors);
            return isValid;
        }

        private bool ValidateDatum(DateTime? value)
        {
            var Validierung = new AktieGekauftValidierung();

            bool isValid = Validierung.ValidateDatum(value, out ICollection<string> validationErrors);

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
            Gesamtbetrag = null;
        }
    }
}
