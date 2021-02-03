using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.AktienModels;
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

namespace Aktien.Logic.Core.Depot
{
    public class DepotAPI
    {
        public void NeueAktieGekauft(double inPreis, double? inFremdkosten, DateTime inDatum, int inAktieID, Double inAnzahl, Data.Types.KaufTypes inKauftyp, Data.Types.OrderTypes inOrderTyp)
        {
            var orderHistoryRepo = new OrderHistoryRepository();
            orderHistoryRepo.Speichern(inPreis, inFremdkosten, inDatum, inAktieID, inAnzahl, inKauftyp, inOrderTyp, BuySell.Buy);

            var DepotAktieRepo = new DepotAktienRepository();
            var depotAktie = DepotAktieRepo.LadeAnhandAktieID(inAktieID);

            if (depotAktie == null)
            {
                depotAktie = new DepotAktie { ID = 0, AktieID = inAktieID, DepotID = 1 };
            }
            var AlteAnzahl = depotAktie.Anzahl;

            depotAktie.Anzahl +=inAnzahl;

            depotAktie.BuyIn = new KaufBerechnungen().BuyInAktieGekauft(depotAktie.BuyIn, AlteAnzahl, depotAktie.Anzahl, inPreis, inAnzahl, inFremdkosten);
            DepotAktieRepo.Speichern(depotAktie.ID, depotAktie.Anzahl, depotAktie.BuyIn, depotAktie.AktieID, depotAktie.DepotID);
        }

        public void EntferneGekaufteAktie(int OrderID)
        {
            var OrderRepo = new OrderHistoryRepository();
            var DepotAktieRepo = new DepotAktienRepository();

            var Order = OrderRepo.LadeByID(OrderID);
            var DepotAktie = DepotAktieRepo.LadeAnhandAktieID(Order.AktieID);

            var AlteAnzahl = DepotAktie.Anzahl;
            DepotAktie.Anzahl -= Order.Anzahl;

            if (DepotAktie.Anzahl == 0)
            {
                DepotAktieRepo.Entfernen(DepotAktie);
            }
            else
            {
                DepotAktie.BuyIn = new KaufBerechnungen().BuyInAktieEntfernt(DepotAktie.BuyIn, AlteAnzahl, DepotAktie.Anzahl, Order.Preis, Order.Anzahl , Order.Fremdkostenzuschlag);
                DepotAktieRepo.Speichern(DepotAktie.ID, DepotAktie.Anzahl, DepotAktie.BuyIn, DepotAktie.AktieID, DepotAktie.DepotID);
            }
            OrderRepo.Entfernen(Order);
        }

        public void NeueAktieVerkauft(double inPreis, double? inFremdkosten, DateTime inDatum, int inAktieID, Double inAnzahl, KaufTypes inKauftyp, OrderTypes inOrderTyp)
        {
            var orderHistoryRepo = new OrderHistoryRepository();
            orderHistoryRepo.Speichern(inPreis, inFremdkosten, inDatum, inAktieID, inAnzahl, inKauftyp, inOrderTyp, BuySell.Sell);

            var DepotAktieRepo = new DepotAktienRepository();
            var DepotAktie = DepotAktieRepo.LadeAnhandAktieID(inAktieID);

            var AlteAnzahl = DepotAktie.Anzahl;
            DepotAktie.Anzahl -= inAnzahl;

            if (DepotAktie.Anzahl == 0)
            {
                DepotAktieRepo.Entfernen(DepotAktie);
            }
            else
            {
                //DepotAktie.BuyIn = ((DepotAktie.BuyIn * AlteAnzahl) - (inPreis * inAnzahl) - inFremdkosten.GetValueOrDefault(0)) / DepotAktie.Anzahl;
                DepotAktieRepo.Speichern(DepotAktie.ID, DepotAktie.Anzahl, DepotAktie.BuyIn, DepotAktie.AktieID, DepotAktie.DepotID);
            }
        }

        public void EntferneVerkaufteAktie(int OrderID)
        {
            var OrderRepo = new OrderHistoryRepository();
            var DepotAktieRepo = new DepotAktienRepository();

            var Order = OrderRepo.LadeByID(OrderID);
            var DepotAktie = DepotAktieRepo.LadeAnhandAktieID(Order.AktieID);

            //var AlteAnzahl = DepotAktie.Anzahl;
            DepotAktie.Anzahl += Order.Anzahl;

            //DepotAktie.BuyIn = ((DepotAktie.BuyIn * AlteAnzahl) + (Order.Preis * Order.Anzahl) + Order.Fremdkostenzuschlag.GetValueOrDefault(0)) / DepotAktie.Anzahl;
            DepotAktieRepo.Speichern(DepotAktie.ID, DepotAktie.Anzahl, DepotAktie.BuyIn, DepotAktie.AktieID, DepotAktie.DepotID);
           
            OrderRepo.Entfernen(Order);
        }
       
        public ObservableCollection<DepotAktie> LadeAlleVorhandeneImDepot()
        {
            return new DepotAktienRepository().LoadAll();
        }

        public void NeueDividendeErhalten(int inAktieID, int inDividendeID, DateTime inDatum, Double? inQuellensteuer, Double? inUmrechnungskurs, int inBestand)
        {
            var Erhaltenedividende = new DividendeErhalten { AktieID = inAktieID, DividendeID = inDividendeID, Bestand = inBestand, Datum = inDatum, Quellensteuer = inQuellensteuer, Umrechnungskurs = inUmrechnungskurs };
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
                new DividendeRepository().Speichern(dividende.ID,dividende.Betrag, dividende.Datum, dividende.AktieID, dividende.Waehrung, dividende.BetragUmgerechnet);
            }

            new DividendeErhaltenRepository().Speichern(null, inDividendeErhalten.Datum, inDividendeErhalten.Quellensteuer, inDividendeErhalten.Umrechnungskurs, inDividendeErhalten.GesamtBrutto, inDividendeErhalten.GesamtNetto, inDividendeErhalten.Bestand, inDividendeErhalten.DividendeID, inDividendeErhalten.AktieID);
        }
  
        public void AktualisiereDividendeErhalten(DividendeErhalten inDividendeErhalten)
        {
            var dividende = new DividendeRepository().LadeAnhandID(inDividendeErhalten.DividendeID);

            inDividendeErhalten.GesamtBrutto = new DividendenBerechnungen().GesamtBrutto(dividende.Betrag, inDividendeErhalten.Bestand);
            inDividendeErhalten.GesamtNetto = new DividendenBerechnungen().GesamtNetto(inDividendeErhalten.GesamtBrutto, inDividendeErhalten.Quellensteuer.GetValueOrDefault(0));

            if ((!dividende.Waehrung.Equals(Waehrungen.Euro)))
            {
                dividende.BetragUmgerechnet = new DividendenBerechnungen().BetragUmgerechnet(dividende.Betrag, inDividendeErhalten.Umrechnungskurs);
                new DividendeRepository().Speichern(dividende.ID, dividende.Betrag, dividende.Datum, dividende.AktieID, dividende.Waehrung, dividende.BetragUmgerechnet);
            }

            new DividendeErhaltenRepository().Speichern(inDividendeErhalten.ID, inDividendeErhalten.Datum, inDividendeErhalten.Quellensteuer, inDividendeErhalten.Umrechnungskurs, inDividendeErhalten.GesamtBrutto, inDividendeErhalten.GesamtNetto, inDividendeErhalten.Bestand, inDividendeErhalten.DividendeID, inDividendeErhalten.AktieID);
        }
    }

    public class AktieSchonVorhandenException : Exception
    { }
}
