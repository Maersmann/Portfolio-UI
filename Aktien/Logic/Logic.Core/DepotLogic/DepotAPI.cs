using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.DepotLogic.Classes;
using Aktien.Logic.Core.DividendeLogic.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aktien.Logic.Core.DepotLogic.Exceptions;
using Aktien.Data.Types.DepotTypes;
using System.Globalization;
using Aktien.Logic.Core.DepotLogic.Models;

namespace Aktien.Logic.Core.Depot
{
    public class DepotAPI
    {
        
        public void WertpapierGekauft(double inPreis, double? inFremdkosten, DateTime inDatum, int inWertpapierID, Double inAnzahl, KaufTypes inKauftyp, OrderTypes inOrderTyp)
        {
            var orderHistory = new OrderHistory { WertpapierID = inWertpapierID, Preis = inPreis, Orderdatum = inDatum, Anzahl = inAnzahl, Fremdkostenzuschlag = inFremdkosten, KaufartTyp = inKauftyp, OrderartTyp = inOrderTyp, BuySell = BuySell.Buy }; 
            new OrderHistoryRepository().Speichern(orderHistory);

            var DepotAktieRepo = new DepotWertpapierRepository();
            var depotAktie = DepotAktieRepo.LadeByWertpapierID(inWertpapierID);

            if (depotAktie == null)
            {
                depotAktie = new DepotWertpapier { ID = 0, WertpapierID = inWertpapierID, DepotID = 1 };
            }
            var AlteAnzahl = depotAktie.Anzahl;

            depotAktie.Anzahl +=inAnzahl;

            depotAktie.BuyIn = new KaufBerechnungen().BuyInAktieGekauft(depotAktie.BuyIn, AlteAnzahl, depotAktie.Anzahl, inPreis, inAnzahl, inFremdkosten);
            DepotAktieRepo.Speichern(depotAktie.ID, depotAktie.Anzahl, depotAktie.BuyIn, depotAktie.WertpapierID, depotAktie.DepotID);

            var Betrag = (inPreis * inAnzahl) + inFremdkosten.GetValueOrDefault(0);
            NeueAusgabe(Betrag, inDatum, AusgabenArtTypes.Kauf, 1, orderHistory.ID, "");
        }

        public void EntferneGekauftenWertpapier(int OrderID)
        {
            var OrderRepo = new OrderHistoryRepository();
            var DepotAktieRepo = new DepotWertpapierRepository();

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

            EntferneAusgabe(null, OrderID, AusgabenArtTypes.Kauf);
            OrderRepo.Entfernen(Order);
        }

        public void WertpapierVerkauft(double inPreis, double? inFremdkosten, DateTime inDatum, int inWertpapierID, Double inAnzahl, KaufTypes inKauftyp, OrderTypes inOrderTyp)
        {
            var DepotAktieRepo = new DepotWertpapierRepository();
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

            var Betrag = (inPreis * inAnzahl) - inFremdkosten.GetValueOrDefault(0);

            NeueEinnahme(Betrag, inDatum, EinnahmeArtTypes.Verkauf, 1, orderHistory.ID, "");
        }

        public void EntferneVerkauftenWertpapier(int OrderID)
        {
            var OrderRepo = new OrderHistoryRepository();
            var DepotAktieRepo = new DepotWertpapierRepository();

            var Order = OrderRepo.LadeByID(OrderID);
            var DepotAktie = DepotAktieRepo.LadeByWertpapierID(Order.WertpapierID);
            
            DepotAktie.Anzahl += Order.Anzahl;

            DepotAktieRepo.Speichern(DepotAktie.ID, DepotAktie.Anzahl, DepotAktie.BuyIn, DepotAktie.WertpapierID, DepotAktie.DepotID);

            EntferneNeueEinnahme(null, OrderID, EinnahmeArtTypes.Verkauf);
            OrderRepo.Entfernen(Order);
            
        }
       
        public ObservableCollection<DepotWertpapier> LadeAlleVorhandeneImDepot()
        {
            new WertpapierRepository().LadeAlle();
            return new DepotWertpapierRepository().LoadAll();
        }
        public ObservableCollection<DepotGesamtUebersichtItem> LadeFuerGesamtUebersicht()
        {
            var returnList = new ObservableCollection<DepotGesamtUebersichtItem>();
            new WertpapierRepository().LadeAlle();
            new DepotWertpapierRepository().LoadAll().ToList().ForEach(item => returnList.Add( new DepotGesamtUebersichtItem { Anzahl = item.Anzahl, BuyIn = item.BuyIn, DepotWertpapierID = item.ID, 
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

            NeueEinnahme(EuroBetrag, inDividendeErhalten.Datum, EinnahmeArtTypes.Dividende, 1, inDividendeErhalten.ID, "");
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

            AktualisiereEinnahme(null, inDividendeErhalten.ID, EuroBetrag, inDividendeErhalten.Datum, EinnahmeArtTypes.Dividende);
            new DividendeErhaltenRepository().Speichern(inDividendeErhalten.ID, inDividendeErhalten.Datum, inDividendeErhalten.Quellensteuer, inDividendeErhalten.Umrechnungskurs, inDividendeErhalten.GesamtBrutto, inDividendeErhalten.GesamtNetto, inDividendeErhalten.Bestand, inDividendeErhalten.DividendeID, inDividendeErhalten.WertpapierID);
        }
    
        public bool WertpapierImDepotVorhanden(int inWertpapierID )
        {
            return new DepotWertpapierRepository().IstWertpapierInDepotVorhanden( inWertpapierID );
        }
  
        public void NeueEinnahme( double inBetrag, DateTime inDatum, EinnahmeArtTypes inTyp, int inDepotID, int? inHerkunftID, string inBeschreibung)
        {
            string Beschreibung = inBeschreibung;

            if ((inBeschreibung.Length == 0 ) && (inHerkunftID.HasValue))
            {
                if ( inTyp.Equals( EinnahmeArtTypes.Dividende ) )
                {
                    var Dividende = new DividendeErhaltenRepository().LadeByID(inHerkunftID.Value);
                    Beschreibung = "Dividende von " + Dividende.Wertpapier.Name;
                }
                else if ( inTyp.Equals( EinnahmeArtTypes.Verkauf ) )
                {
                    var Verkauf = new OrderHistoryRepository().LadeByID(inHerkunftID.Value);
                    Beschreibung = "Verkauf von " + Verkauf.Wertpapier.Name;
                }
            }
            var Einnahme = new Einnahme { Art = inTyp, Betrag = inBetrag, DepotID = inDepotID, Datum = inDatum, HerkunftID = inHerkunftID, Beschreibung = Beschreibung };
            new EinnahmenRepository().Speichern(Einnahme);

            var Depot = new DepotRepository().LoadByID(inDepotID);
            if (Depot.GesamtEinahmen == null) Depot.GesamtEinahmen = 0; 
            Depot.GesamtEinahmen += inBetrag;
            new DepotRepository().Speichern(Depot);
        }

        public void EntferneNeueEinnahme(int? inID, int? inHerkunftID, EinnahmeArtTypes? inTyp)
        {
            Einnahme einnahme = null;
            if ( inID.HasValue )
            {
                einnahme = new EinnahmenRepository().LoadByID(inID.Value);
            }
            else if (inHerkunftID.HasValue)
            {
                einnahme = new EinnahmenRepository().LoadByHerkunftIDAndArt(inHerkunftID.Value, inTyp.Value);
            }

            if (einnahme == null) return;

            var Depot = new DepotRepository().LoadByID(einnahme.DepotID);
            Depot.GesamtEinahmen -= einnahme.Betrag;
            new DepotRepository().Speichern(Depot);
            new EinnahmenRepository().Entfernen(einnahme);
        }
        
        public void AktualisiereEinnahme(int? inID, int? inHerkunftID, double inBetrag, DateTime inDatum, EinnahmeArtTypes inTyp)
        {
            Einnahme einnahme = null;
            if (inID.HasValue)
            {
                einnahme = new EinnahmenRepository().LoadByID(inID.Value);
            }
            else if (inHerkunftID.HasValue)
            {
                einnahme = new EinnahmenRepository().LoadByHerkunftIDAndArt(inHerkunftID.Value, inTyp);
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
    
        public void NeueAusgabe(double inBetrag, DateTime inDatum, AusgabenArtTypes inTyp, int inDepotID, int? inHerkunftID, string inBeschreibung)
        {
            string Beschreibung = inBeschreibung;

            if ((Beschreibung.Length == 0) && (inHerkunftID.HasValue))
            {
                if (inTyp.Equals(AusgabenArtTypes.Kauf))
                {
                    var Dividende = new OrderHistoryRepository().LadeByID(inHerkunftID.Value);
                    Beschreibung = "Kauf von " + Dividende.Wertpapier.Name;
                }
            }
            var Ausgabe = new Ausgabe { Art = inTyp, Betrag = inBetrag, DepotID = inDepotID, Datum = inDatum, HerkunftID = inHerkunftID, Beschreibung = Beschreibung };
            new AusgabenRepository().Speichern(Ausgabe);

            var Depot = new DepotRepository().LoadByID(inDepotID);
            if (Depot.GesamtAusgaben == null) Depot.GesamtAusgaben = 0;
            Depot.GesamtAusgaben += inBetrag;
            new DepotRepository().Speichern(Depot);
        }
   
        public void EntferneAusgabe(int? inID, int? inHerkunftID, AusgabenArtTypes? inTyp)
        {
            Ausgabe ausgabe = null;
            if (inID.HasValue)
            {
                ausgabe = new AusgabenRepository().LoadByID(inID.Value);
            }
            else if (inHerkunftID.HasValue)
            {
                ausgabe = new AusgabenRepository().LoadByHerkunftIDAndArt(inHerkunftID.Value, inTyp.Value);
            }

            if (ausgabe == null) return;

            var Depot = new DepotRepository().LoadByID(ausgabe.DepotID);
            Depot.GesamtAusgaben -= ausgabe.Betrag;
            new DepotRepository().Speichern(Depot);
            new AusgabenRepository().Entfernen(ausgabe);
        }

        public void AktualisiereAusgabe(int? inID, int? inHerkunftID, double inBetrag, DateTime inDatum, AusgabenArtTypes inTyp)
        {
            Ausgabe ausgabe = null;
            if (inID.HasValue)
            {
                ausgabe = new AusgabenRepository().LoadByID(inID.Value);
            }
            else if (inHerkunftID.HasValue)
            {
                ausgabe = new AusgabenRepository().LoadByHerkunftIDAndArt(inHerkunftID.Value, inTyp);
            }

            if (ausgabe == null) return;

            var Depot = new DepotRepository().LoadByID(ausgabe.DepotID);
            Depot.GesamtAusgaben -= ausgabe.Betrag;

            ausgabe.Datum = inDatum;
            ausgabe.Art = inTyp;
            ausgabe.Betrag = inBetrag;

            Depot.GesamtAusgaben += ausgabe.Betrag;


            new DepotRepository().Speichern(Depot);
            new AusgabenRepository().Speichern(ausgabe);
        }
    }
}
