using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.WertpapierModels;
using Aktien.Data.Model.DepotModels;
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

namespace Aktien.Logic.Core.Depot
{
    public class DepotAPI
    {
        public Exception ZuVieleWertpapiereVerkauftException { get; private set; }

        public void NeuerWertpapierGekauft(double inPreis, double? inFremdkosten, DateTime inDatum, int inWertpapierID, Double inAnzahl, Data.Types.KaufTypes inKauftyp, Data.Types.OrderTypes inOrderTyp)
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

        public void NeuerWertpapierVerkauft(double inPreis, double? inFremdkosten, DateTime inDatum, int inWertpapierID, Double inAnzahl, KaufTypes inKauftyp, OrderTypes inOrderTyp)
        {
            var DepotAktieRepo = new DepotAktienRepository();
            var DepotAktie = DepotAktieRepo.LadeByWertpapierID(inWertpapierID);

            if (DepotAktie.Anzahl < inAnzahl)
                throw new ZuVieleWertpapiereVerkaufException();

            var orderHistoryRepo = new OrderHistoryRepository();       
            orderHistoryRepo.Speichern(inPreis, inFremdkosten, inDatum, inWertpapierID, inAnzahl, inKauftyp, inOrderTyp, BuySell.Sell);

            DepotAktie.Anzahl -= inAnzahl;

            if (DepotAktie.Anzahl == 0)
            {
                DepotAktieRepo.Entfernen(DepotAktie);
            }
            else
            {
                //DepotAktie.BuyIn = ((DepotAktie.BuyIn * AlteAnzahl) - (inPreis * inAnzahl) - inFremdkosten.GetValueOrDefault(0)) / DepotAktie.Anzahl;
                DepotAktieRepo.Speichern(DepotAktie.ID, DepotAktie.Anzahl, DepotAktie.BuyIn, DepotAktie.WertpapierID, DepotAktie.DepotID);
            }
        }

        public void EntferneVerkauftenWertpapier(int OrderID)
        {
            var OrderRepo = new OrderHistoryRepository();
            var DepotAktieRepo = new DepotAktienRepository();

            var Order = OrderRepo.LadeByID(OrderID);
            var DepotAktie = DepotAktieRepo.LadeByWertpapierID(Order.WertpapierID);

            //var AlteAnzahl = DepotAktie.Anzahl;
            DepotAktie.Anzahl += Order.Anzahl;

            //DepotAktie.BuyIn = ((DepotAktie.BuyIn * AlteAnzahl) + (Order.Preis * Order.Anzahl) + Order.Fremdkostenzuschlag.GetValueOrDefault(0)) / DepotAktie.Anzahl;
            DepotAktieRepo.Speichern(DepotAktie.ID, DepotAktie.Anzahl, DepotAktie.BuyIn, DepotAktie.WertpapierID, DepotAktie.DepotID);
           
            OrderRepo.Entfernen(Order);
        }
       
        public ObservableCollection<DepotWertpapier> LadeAlleVorhandeneImDepot()
        {
            new WertpapierRepository().LadeAlleETFs();
            return new DepotAktienRepository().LoadAll();
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

            new DividendeErhaltenRepository().Speichern(null, inDividendeErhalten.Datum, inDividendeErhalten.Quellensteuer, inDividendeErhalten.Umrechnungskurs, inDividendeErhalten.GesamtBrutto, inDividendeErhalten.GesamtNetto, inDividendeErhalten.Bestand, inDividendeErhalten.DividendeID, inDividendeErhalten.WertpapierID);
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

            new DividendeErhaltenRepository().Speichern(inDividendeErhalten.ID, inDividendeErhalten.Datum, inDividendeErhalten.Quellensteuer, inDividendeErhalten.Umrechnungskurs, inDividendeErhalten.GesamtBrutto, inDividendeErhalten.GesamtNetto, inDividendeErhalten.Bestand, inDividendeErhalten.DividendeID, inDividendeErhalten.WertpapierID);
        }
    
        public bool WertpapierImDepotVorhanden(int inWertpapierID )
        {
            return new DepotAktienRepository().IstAktieInDepotVorhanden( inWertpapierID );
        }
    }
}
