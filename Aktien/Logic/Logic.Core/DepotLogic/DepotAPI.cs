using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.Depot.Classes;
using Aktien.Logic.Core.DividendeLogic.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aktien.Logic.Core.DepotLogic.Exceptions;
using Aktien.Data.Model.DepotModels;
using Aktien.Data.Types.DepotTypes;

namespace Aktien.Logic.Core.Depot
{
    public class DepotAPI
    {
        public Exception ZuVieleWertpapiereVerkauftException { get; private set; }

        public void WertpapierGekauft(double inPreis, double? inFremdkosten, DateTime inDatum, int inWertpapierID, Double inAnzahl, Data.Types.KaufTypes inKauftyp, Data.Types.OrderTypes inOrderTyp)
        {
            var orderHistoryRepo = new OrderHistoryRepository();
            orderHistoryRepo.Speichern(inPreis, inFremdkosten, inDatum, inWertpapierID, inAnzahl, inKauftyp, inOrderTyp, BuySell.Buy);

            var DepotAktieRepo = new DepotAktienRepository();
            var depotAktie = DepotAktieRepo.LadeByWertpapierID(inWertpapierID);

            if (depotAktie == null)
            {
                depotAktie = new DepotWertpapier { ID = 0, WertpapierID = inWertpapierID, DepotID = 1 };
            }
            var AlteAnzahl = depotAktie.Anzahl;

            depotAktie.Anzahl +=inAnzahl;

            depotAktie.BuyIn = new KaufBerechnungen().BuyInAktieGekauft(depotAktie.BuyIn, AlteAnzahl, depotAktie.Anzahl, inPreis, inAnzahl, inFremdkosten);
            DepotAktieRepo.Speichern(depotAktie.ID, depotAktie.Anzahl, depotAktie.BuyIn, depotAktie.WertpapierID, depotAktie.DepotID);

        }

        public void EntferneGekauftenWertpapier(int OrderID)
        {
            var OrderRepo = new OrderHistoryRepository();
            var DepotAktieRepo = new DepotAktienRepository();

            var Order = OrderRepo.LadeByID(OrderID);
            var DepotAktie = DepotAktieRepo.LadeByWertpapierID(Order.WertpapierID);

            var AlteAnzahl = DepotAktie.Anzahl;
            DepotAktie.Anzahl -= Order.Anzahl;

            if (DepotAktie.Anzahl == 0)
            {
                DepotAktieRepo.Entfernen(DepotAktie);
            }
            else
            {
                DepotAktie.BuyIn = new KaufBerechnungen().BuyInAktieEntfernt(DepotAktie.BuyIn, AlteAnzahl, DepotAktie.Anzahl, Order.Preis, Order.Anzahl , Order.Fremdkostenzuschlag);
                DepotAktieRepo.Speichern(DepotAktie.ID, DepotAktie.Anzahl, DepotAktie.BuyIn, DepotAktie.WertpapierID, DepotAktie.DepotID);
            }
            OrderRepo.Entfernen(Order);
        }

        public void WertpapierVerkauft(double inPreis, double? inFremdkosten, DateTime inDatum, int inWertpapierID, Double inAnzahl, KaufTypes inKauftyp, OrderTypes inOrderTyp)
        {
            var DepotAktieRepo = new DepotAktienRepository();
            var DepotAktie = DepotAktieRepo.LadeByWertpapierID(inWertpapierID);

            if (DepotAktie.Anzahl < inAnzahl)
                throw new ZuVieleWertpapiereVerkaufException();

            var orderHistory = new OrderHistory { WertpapierID = inWertpapierID, Preis = inPreis, Orderdatum = inDatum, Anzahl = inAnzahl, Fremdkostenzuschlag = inFremdkosten, KaufartTyp = inKauftyp, OrderartTyp = inOrderTyp, BuySell = BuySell.Sell };
            new OrderHistoryRepository().Speichern(orderHistory);

            DepotAktie.Anzahl -= inAnzahl;

            if (DepotAktie.Anzahl == 0)
            {
                DepotAktieRepo.Entfernen(DepotAktie);
            }
            else
            {
                DepotAktieRepo.Speichern(DepotAktie.ID, DepotAktie.Anzahl, DepotAktie.BuyIn, DepotAktie.WertpapierID, DepotAktie.DepotID);
            }

            var Betrag = (inPreis * inAnzahl) + inFremdkosten.GetValueOrDefault(0);

            NeueEinnahme(Betrag, inDatum, EinnahmeArtTypes.Verkauf, 1, orderHistory.ID);
        }

        public void EntferneVerkauftenWertpapier(int OrderID)
        {
            var OrderRepo = new OrderHistoryRepository();
            var DepotAktieRepo = new DepotAktienRepository();

            var Order = OrderRepo.LadeByID(OrderID);
            var DepotAktie = DepotAktieRepo.LadeByWertpapierID(Order.WertpapierID);
            
            DepotAktie.Anzahl += Order.Anzahl;

            DepotAktieRepo.Speichern(DepotAktie.ID, DepotAktie.Anzahl, DepotAktie.BuyIn, DepotAktie.WertpapierID, DepotAktie.DepotID);

            EntferneNeueEinnahme(null, OrderID);
            OrderRepo.Entfernen(Order);
            
        }
       
        public ObservableCollection<DepotWertpapier> LadeAlleVorhandeneImDepot()
        {
            new WertpapierRepository().LadeAlle();
            return new DepotAktienRepository().LoadAll();
        }
        public ObservableCollection<DepotGesamtUebersichtItem> LadeFuerGesamtUebersicht()
        {
            var returnList = new ObservableCollection<DepotGesamtUebersichtItem>();
            new WertpapierRepository().LadeAlle();
            new DepotAktienRepository().LoadAll().ToList().ForEach(item => returnList.Add( new DepotGesamtUebersichtItem { Anzahl = item.Anzahl, BuyIn = item.BuyIn, DepotWertpapierID = item.ID, 
                                                                                                                           WertpapierID = item.WertpapierID, WertpapierTyp = item.Wertpapier.WertpapierTyp,
                                                                                                                           Bezeichnung = item.Wertpapier.Name }));
            return returnList;
        }

        public void NeueDividendeErhalten(int inWertpapierID, int inDividendeID, DateTime inDatum, Double? inQuellensteuer, Double? inUmrechnungskurs, int inBestand)
        {
            var Erhaltenedividende = new DividendeErhalten { WertpapierID = inWertpapierID, DividendeID = inDividendeID, Bestand = inBestand, Datum = inDatum, Quellensteuer = inQuellensteuer, Umrechnungskurs = inUmrechnungskurs };
            NeueDividendeErhalten(Erhaltenedividende);
        }

        public void NeueDividendeErhalten(DividendeErhalten inDividendeErhalten)
        {
            var dividende = new DividendeRepository().LadeAnhandID(inDividendeErhalten.DividendeID);

            inDividendeErhalten.GesamtBrutto = new DividendenBerechnungen().GesamtBrutto(dividende.Betrag, inDividendeErhalten.Bestand);
            inDividendeErhalten.GesamtNetto = new DividendenBerechnungen().GesamtNetto(inDividendeErhalten.GesamtBrutto, inDividendeErhalten.Quellensteuer.GetValueOrDefault(0));

            if( (!dividende.Waehrung.Equals(Waehrungen.Euro)) && ( !dividende.BetragUmgerechnet.HasValue ))
            {
                dividende.BetragUmgerechnet = new DividendenBerechnungen().BetragUmgerechnet(dividende.Betrag, inDividendeErhalten.Umrechnungskurs);
                new DividendeRepository().Speichern(dividende.ID,dividende.Betrag, dividende.Datum, dividende.WertpapierID, dividende.Waehrung, dividende.BetragUmgerechnet);
            }

            var EuroBetrag = inDividendeErhalten.GesamtNetto;
            if (!dividende.Waehrung.Equals(Waehrungen.Euro))
            {
                EuroBetrag = new DividendenBerechnungen().BetragUmgerechnet(EuroBetrag, inDividendeErhalten.Umrechnungskurs);
            }

            new DividendeErhaltenRepository().Speichern(inDividendeErhalten);

            NeueEinnahme(EuroBetrag, inDividendeErhalten.Datum, EinnahmeArtTypes.Dividende, 1, inDividendeErhalten.ID);
        }
  
        public void AktualisiereDividendeErhalten(DividendeErhalten inDividendeErhalten)
        {
            var dividende = new DividendeRepository().LadeAnhandID(inDividendeErhalten.DividendeID);

            inDividendeErhalten.GesamtBrutto = new DividendenBerechnungen().GesamtBrutto(dividende.Betrag, inDividendeErhalten.Bestand);
            inDividendeErhalten.GesamtNetto = new DividendenBerechnungen().GesamtNetto(inDividendeErhalten.GesamtBrutto, inDividendeErhalten.Quellensteuer.GetValueOrDefault(0));

            if ((!dividende.Waehrung.Equals(Waehrungen.Euro)))
            {
                dividende.BetragUmgerechnet = new DividendenBerechnungen().BetragUmgerechnet(dividende.Betrag, inDividendeErhalten.Umrechnungskurs);
                new DividendeRepository().Speichern(dividende.ID, dividende.Betrag, dividende.Datum, dividende.WertpapierID, dividende.Waehrung, dividende.BetragUmgerechnet);
            }

            var EuroBetrag = inDividendeErhalten.GesamtNetto;
            if (!dividende.Waehrung.Equals(Waehrungen.Euro))
            {
                EuroBetrag = new DividendenBerechnungen().BetragUmgerechnet(EuroBetrag, inDividendeErhalten.Umrechnungskurs);
            }

            AktualisiereNeueEinnahme(null, inDividendeErhalten.ID, EuroBetrag, inDividendeErhalten.Datum, EinnahmeArtTypes.Dividende);
            new DividendeErhaltenRepository().Speichern(inDividendeErhalten.ID, inDividendeErhalten.Datum, inDividendeErhalten.Quellensteuer, inDividendeErhalten.Umrechnungskurs, inDividendeErhalten.GesamtBrutto, inDividendeErhalten.GesamtNetto, inDividendeErhalten.Bestand, inDividendeErhalten.DividendeID, inDividendeErhalten.WertpapierID);
        }
    
        public bool WertpapierImDepotVorhanden(int inWertpapierID )
        {
            return new DepotAktienRepository().IstAktieInDepotVorhanden( inWertpapierID );
        }
  
        public void NeueEinnahme( double inBetrag, DateTime inDatum, EinnahmeArtTypes inTyp, int inDepotID, int? inHerkunftID)
        {
            var Einnahme = new Einnahme { Art = inTyp, Betrag = inBetrag, DepotID = inDepotID, Datum = inDatum, HerkunftID = inHerkunftID };
            new EinnahmenRepository().Speichern(Einnahme);

            var Depot = new DepotRepository().LoadByID(inDepotID);
            if (Depot.GesamtEinahmen == null) Depot.GesamtEinahmen = 0; 
            Depot.GesamtEinahmen += inBetrag;
            new DepotRepository().Speichern(Depot);
        }

        public void EntferneNeueEinnahme(int? inID, int? inHerkunftID)
        {
            Einnahme einnahme = null;
            if ( inID.HasValue )
            {
                einnahme = new EinnahmenRepository().LoadByID(inID.Value);
            }
            else if (inHerkunftID.HasValue)
            {
                einnahme = new EinnahmenRepository().LoadByHerkunftID(inHerkunftID.Value);
            }

            if (einnahme == null) return;

            var Depot = new DepotRepository().LoadByID(einnahme.DepotID);
            Depot.GesamtEinahmen -= einnahme.Betrag;
            new DepotRepository().Speichern(Depot);
            new EinnahmenRepository().Entfernen(einnahme);
        }
        
        public void AktualisiereNeueEinnahme(int? inID, int? inHerkunftID, double inBetrag, DateTime inDatum, EinnahmeArtTypes inTyp)
        {
            Einnahme einnahme = null;
            if (inID.HasValue)
            {
                einnahme = new EinnahmenRepository().LoadByID(inID.Value);
            }
            else if (inHerkunftID.HasValue)
            {
                einnahme = new EinnahmenRepository().LoadByHerkunftID(inHerkunftID.Value);
            }

            if (einnahme == null) return;

            var Depot = new DepotRepository().LoadByID(einnahme.DepotID);
            Depot.GesamtEinahmen -= einnahme.Betrag;

            einnahme.Datum = inDatum;
            einnahme.Art = inTyp;
            einnahme.Betrag = inBetrag;

            Depot.GesamtEinahmen += einnahme.Betrag;


            new DepotRepository().Speichern(Depot);
            new EinnahmenRepository().Speichern(einnahme);
        }
    
    }
}
