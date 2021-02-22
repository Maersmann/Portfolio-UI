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
using Aktien.Logic.Core.WertpapierLogic.Exceptions;

namespace Aktien.Logic.Core.Depot
{
    public class DepotAPI
    {
        
        public void WertpapierGekauft(double preis, double? fremdkosten, DateTime datum, int wertpapierID, Double anzahl, KaufTypes kauftyp, OrderTypes orderTyp, Double? gesamtbetrag)
        {
            if (new OrderHistoryRepository().IstNeuereOrderVorhanden(wertpapierID, datum)) throw new NeuereOrderVorhandenException();

            if (gesamtbetrag.HasValue)
                preis = Math.Round(gesamtbetrag.Value / anzahl, 3, MidpointRounding.AwayFromZero);

            var orderHistory = new OrderHistory { WertpapierID = wertpapierID, Preis = preis, Orderdatum = datum, Anzahl = anzahl, Fremdkostenzuschlag = fremdkosten, KaufartTyp = kauftyp, OrderartTyp = orderTyp, BuySell = BuySell.Buy }; 
            new OrderHistoryRepository().Speichern(orderHistory);

            var DepotAktieRepo = new DepotWertpapierRepository();
            var depotAktie = DepotAktieRepo.LadeByWertpapierID(wertpapierID);

            if (depotAktie == null)
            {
                depotAktie = new DepotWertpapier { ID = 0, WertpapierID = wertpapierID, DepotID = 1 };
            }
            var AlteAnzahl = depotAktie.Anzahl;

            depotAktie.Anzahl +=anzahl;

            

            depotAktie.BuyIn = new KaufBerechnungen().BuyInAktieGekauft(depotAktie.BuyIn, AlteAnzahl, depotAktie.Anzahl, preis, anzahl, fremdkosten);
            DepotAktieRepo.Speichern(depotAktie.ID, depotAktie.Anzahl, depotAktie.BuyIn, depotAktie.WertpapierID, depotAktie.DepotID);

            var Betrag = (preis * anzahl) + fremdkosten.GetValueOrDefault(0);
            NeueAusgabe(Betrag, datum, AusgabenArtTypes.Kauf, 1, orderHistory.ID, "");
        }

        public void EntferneGekauftenWertpapier(int orderID)
        {
            var OrderRepo = new OrderHistoryRepository();
            var DepotAktieRepo = new DepotWertpapierRepository();

            var Order = OrderRepo.LadeByID(orderID);
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

            EntferneAusgabe(null, orderID, AusgabenArtTypes.Kauf);
            OrderRepo.Entfernen(Order);
        }

        public void WertpapierVerkauft(double preis, double? fremdkosten, DateTime datum, int wertpapierID, Double anzahl, KaufTypes kauftyp, OrderTypes orderTyp, Double? gesamtbetrag)
        {
            if (new OrderHistoryRepository().IstNeuereOrderVorhanden(wertpapierID, datum)) throw new NeuereOrderVorhandenException();

            if (gesamtbetrag.HasValue)
                preis = Math.Round(gesamtbetrag.Value / anzahl, 3, MidpointRounding.AwayFromZero);

            var DepotAktieRepo = new DepotWertpapierRepository();
            var DepotAktie = DepotAktieRepo.LadeByWertpapierID(wertpapierID);

            if (DepotAktie.Anzahl < anzahl)
                throw new ZuVieleWertpapiereVerkaufException();

            var orderHistory = new OrderHistory { WertpapierID = wertpapierID, Preis = preis, Orderdatum = datum, Anzahl = anzahl, Fremdkostenzuschlag = fremdkosten, KaufartTyp = kauftyp, OrderartTyp = orderTyp, BuySell = BuySell.Sell };
            new OrderHistoryRepository().Speichern(orderHistory);

            DepotAktie.Anzahl -= anzahl;

            if (DepotAktie.Anzahl == 0)
            {
                DepotAktieRepo.Entfernen(DepotAktie);
            }
            else
            {
                DepotAktieRepo.Speichern(DepotAktie.ID, DepotAktie.Anzahl, DepotAktie.BuyIn, DepotAktie.WertpapierID, DepotAktie.DepotID);
            }

            var Betrag = (preis * anzahl) - fremdkosten.GetValueOrDefault(0);

            NeueEinnahme(Betrag, datum, EinnahmeArtTypes.Verkauf, 1, orderHistory.ID, "");
        }

        public void EntferneVerkauftenWertpapier(int orderID)
        {
            var OrderRepo = new OrderHistoryRepository();
            var DepotAktieRepo = new DepotWertpapierRepository();

            var Order = OrderRepo.LadeByID(orderID);
            var DepotAktie = DepotAktieRepo.LadeByWertpapierID(Order.WertpapierID);

            if (DepotAktie != null)
            {
                DepotAktie.Anzahl += Order.Anzahl;
                DepotAktieRepo.Speichern(DepotAktie.ID, DepotAktie.Anzahl, DepotAktie.BuyIn, DepotAktie.WertpapierID, DepotAktie.DepotID);
            }

            EntferneNeueEinnahme(null, orderID, EinnahmeArtTypes.Verkauf);
            OrderRepo.Entfernen(Order);

            if(DepotAktie == null)
                new DepotWertpapierFunctions().NeuBerechnen(Order.WertpapierID);

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

        public void NeueDividendeErhalten(DividendeErhalten dividendeErhalten)
        {
            var dividende = new DividendeRepository().LadeAnhandID(dividendeErhalten.DividendeID);

            dividendeErhalten.GesamtBrutto = new DividendenBerechnungen().GesamtBrutto(dividende.Betrag, dividendeErhalten.Bestand);
            dividendeErhalten.GesamtNetto = new DividendenBerechnungen().GesamtNetto(dividendeErhalten.GesamtBrutto, dividendeErhalten.Quellensteuer.GetValueOrDefault(0));

            if( (!dividende.Waehrung.Equals(Waehrungen.Euro)) && ( !dividende.BetragUmgerechnet.HasValue ))
            {
                dividende.BetragUmgerechnet = new DividendenBerechnungen().BetragUmgerechnet(dividende.Betrag, dividendeErhalten.Umrechnungskurs,true, dividende.RundungArt);
                new DividendeRepository().Speichern(dividende.ID,dividende.Betrag, dividende.Datum, dividende.WertpapierID, dividende.Waehrung, dividende.BetragUmgerechnet, dividende.RundungArt);
            }

            var Eurobetrag = dividendeErhalten.GesamtNetto;
            if (!dividendeErhalten.GesamtNettoUmgerechnetErhalten.HasValue)
            {      
                if (!dividende.Waehrung.Equals(Waehrungen.Euro))
                {
                    dividendeErhalten.GesamtNettoUmgerechnetErhalten = new DividendenBerechnungen().BetragUmgerechnet(dividendeErhalten.GesamtNetto, dividendeErhalten.Umrechnungskurs,true, dividendeErhalten.RundungArt);
                    dividendeErhalten.GesamtNettoUmgerechnetErmittelt = new DividendenBerechnungen().BetragUmgerechnet(dividendeErhalten.GesamtNetto, dividendeErhalten.Umrechnungskurs, false, dividendeErhalten.RundungArt);
                    Eurobetrag = dividendeErhalten.GesamtNettoUmgerechnetErhalten.Value;
                }
            }
            else
                Eurobetrag = dividendeErhalten.GesamtNettoUmgerechnetErhalten.Value;

            new DividendeErhaltenRepository().Speichern(dividendeErhalten);

            NeueEinnahme(Eurobetrag, dividendeErhalten.Datum, EinnahmeArtTypes.Dividende, 1, dividendeErhalten.ID, "");
        }
  
        public void AktualisiereDividendeErhalten(DividendeErhalten dividendeErhalten)
        {
            var dividende = new DividendeRepository().LadeAnhandID(dividendeErhalten.DividendeID);

            dividendeErhalten.GesamtBrutto = new DividendenBerechnungen().GesamtBrutto(dividende.Betrag, dividendeErhalten.Bestand);
            dividendeErhalten.GesamtNetto = new DividendenBerechnungen().GesamtNetto(dividendeErhalten.GesamtBrutto, dividendeErhalten.Quellensteuer.GetValueOrDefault(0));

            if ((!dividende.Waehrung.Equals(Waehrungen.Euro)))
            {
                dividende.BetragUmgerechnet = new DividendenBerechnungen().BetragUmgerechnet(dividende.Betrag, dividendeErhalten.Umrechnungskurs,true, dividende.RundungArt);
                new DividendeRepository().Speichern(dividende.ID, dividende.Betrag, dividende.Datum, dividende.WertpapierID, dividende.Waehrung, dividende.BetragUmgerechnet, dividende.RundungArt);
            }

            var EuroBetrag = dividendeErhalten.GesamtNetto;
            if (!dividende.Waehrung.Equals(Waehrungen.Euro))
            {
                EuroBetrag = new DividendenBerechnungen().BetragUmgerechnet(EuroBetrag, dividendeErhalten.Umrechnungskurs,true, dividendeErhalten.RundungArt);
            }

            AktualisiereEinnahme(null, dividendeErhalten.ID, EuroBetrag, dividendeErhalten.Datum, EinnahmeArtTypes.Dividende);
            new DividendeErhaltenRepository().Speichern(dividendeErhalten.ID, dividendeErhalten.Datum, dividendeErhalten.Quellensteuer, dividendeErhalten.Umrechnungskurs, dividendeErhalten.GesamtBrutto, dividendeErhalten.GesamtNetto, dividendeErhalten.Bestand, 
                                                        dividendeErhalten.DividendeID, dividendeErhalten.WertpapierID, dividendeErhalten.RundungArt, dividendeErhalten.GesamtNettoUmgerechnetErhalten, dividendeErhalten.GesamtNettoUmgerechnetErmittelt);
        }
    
        public bool WertpapierImDepotVorhanden(int wertpapierID )
        {
            return new DepotWertpapierRepository().IstWertpapierInDepotVorhanden( wertpapierID );
        }
  
        public void NeueEinnahme( double betrag, DateTime datum, EinnahmeArtTypes typ, int depotID, int? herkunftID, string beschreibung)
        {
            string Beschreibung = beschreibung;

            if ((beschreibung.Length == 0 ) && (herkunftID.HasValue))
            {
                if ( typ.Equals( EinnahmeArtTypes.Dividende ) )
                {
                    var Dividende = new DividendeErhaltenRepository().LadeByID(herkunftID.Value);
                    Beschreibung = "Dividende von " + Dividende.Wertpapier.Name;
                }
                else if ( typ.Equals( EinnahmeArtTypes.Verkauf ) )
                {
                    var Verkauf = new OrderHistoryRepository().LadeByID(herkunftID.Value);
                    Beschreibung = "Verkauf von " + Verkauf.Wertpapier.Name;
                }
            }
            var Einnahme = new Einnahme { Art = typ, Betrag = betrag, DepotID = depotID, Datum = datum, HerkunftID = herkunftID, Beschreibung = Beschreibung };
            new EinnahmenRepository().Speichern(Einnahme);

            var Depot = new DepotRepository().LoadByID(depotID);
            if (Depot.GesamtEinahmen == null) Depot.GesamtEinahmen = 0; 
            Depot.GesamtEinahmen += betrag;
            new DepotRepository().Speichern(Depot);
        }

        public void EntferneNeueEinnahme(int? id, int? herkunftID, EinnahmeArtTypes? typ)
        {
            Einnahme einnahme = null;
            if ( id.HasValue )
            {
                einnahme = new EinnahmenRepository().LoadByID(id.Value);
            }
            else if (herkunftID.HasValue)
            {
                einnahme = new EinnahmenRepository().LoadByHerkunftIDAndArt(herkunftID.Value, typ.Value);
            }

            if (einnahme == null) return;

            var Depot = new DepotRepository().LoadByID(einnahme.DepotID);
            Depot.GesamtEinahmen -= einnahme.Betrag;
            new DepotRepository().Speichern(Depot);
            new EinnahmenRepository().Entfernen(einnahme);
        }
        
        public void AktualisiereEinnahme(int? id, int? herkunftID, double betrag, DateTime datum, EinnahmeArtTypes typ)
        {
            Einnahme einnahme = null;
            if (id.HasValue)
            {
                einnahme = new EinnahmenRepository().LoadByID(id.Value);
            }
            else if (herkunftID.HasValue)
            {
                einnahme = new EinnahmenRepository().LoadByHerkunftIDAndArt(herkunftID.Value, typ);
            }

            if (einnahme == null) return;

            var Depot = new DepotRepository().LoadByID(einnahme.DepotID);
            Depot.GesamtEinahmen -= einnahme.Betrag;

            einnahme.Datum = datum;
            einnahme.Art = typ;
            einnahme.Betrag = betrag;

            Depot.GesamtEinahmen += einnahme.Betrag;


            new DepotRepository().Speichern(Depot);
            new EinnahmenRepository().Speichern(einnahme);
        }
    
        public void NeueAusgabe(double betrag, DateTime datum, AusgabenArtTypes typ, int depotID, int? herkunftID, string beschreibung)
        {
            string Beschreibung = beschreibung;

            if ((Beschreibung.Length == 0) && (herkunftID.HasValue))
            {
                if (typ.Equals(AusgabenArtTypes.Kauf))
                {
                    var Dividende = new OrderHistoryRepository().LadeByID(herkunftID.Value);
                    Beschreibung = "Kauf von " + Dividende.Wertpapier.Name;
                }
            }
            var Ausgabe = new Ausgabe { Art = typ, Betrag = betrag, DepotID = depotID, Datum = datum, HerkunftID = herkunftID, Beschreibung = Beschreibung };
            new AusgabenRepository().Speichern(Ausgabe);

            var Depot = new DepotRepository().LoadByID(depotID);
            if (Depot.GesamtAusgaben == null) Depot.GesamtAusgaben = 0;
            Depot.GesamtAusgaben += betrag;
            new DepotRepository().Speichern(Depot);
        }
   
        public void EntferneAusgabe(int? id, int? herkunftID, AusgabenArtTypes? typ)
        {
            Ausgabe ausgabe = null;
            if (id.HasValue)
            {
                ausgabe = new AusgabenRepository().LoadByID(id.Value);
            }
            else if (herkunftID.HasValue)
            {
                ausgabe = new AusgabenRepository().LoadByHerkunftIDAndArt(herkunftID.Value, typ.Value);
            }

            if (ausgabe == null) return;

            var Depot = new DepotRepository().LoadByID(ausgabe.DepotID);
            Depot.GesamtAusgaben -= ausgabe.Betrag;
            new DepotRepository().Speichern(Depot);
            new AusgabenRepository().Entfernen(ausgabe);
        }

        public void AktualisiereAusgabe(int? id, int? herkunftID, double betrag, DateTime datum, AusgabenArtTypes typ)
        {
            Ausgabe ausgabe = null;
            if (id.HasValue)
            {
                ausgabe = new AusgabenRepository().LoadByID(id.Value);
            }
            else if (herkunftID.HasValue)
            {
                ausgabe = new AusgabenRepository().LoadByHerkunftIDAndArt(herkunftID.Value, typ);
            }

            if (ausgabe == null) return;

            var Depot = new DepotRepository().LoadByID(ausgabe.DepotID);
            Depot.GesamtAusgaben -= ausgabe.Betrag;

            ausgabe.Datum = datum;
            ausgabe.Art = typ;
            ausgabe.Betrag = betrag;

            Depot.GesamtAusgaben += ausgabe.Betrag;


            new DepotRepository().Speichern(Depot);
            new AusgabenRepository().Speichern(ausgabe);
        }
    }
}
