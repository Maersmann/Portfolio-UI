using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.AktienModels;
using Aktien.Data.Model.DepotModels;
using Aktien.Data.Types;
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
        public void NeuAktieGekauft(double inPreis, double? inFremdkosten, DateTime inDatum, int inAktieID, int inAnzahl, Data.Types.KaufTypes inKauftyp, Data.Types.OrderTypes inOrderTyp)
        {
            var orderHistoryRepo = new OrderHistoryRepository();
            orderHistoryRepo.Speichern(inPreis, inFremdkosten, inDatum, inAktieID, inAnzahl, inKauftyp, inOrderTyp, BuySell.Buy);

            var DepotAktieRepo = new DepotAktienRepository();
            var depotAktie = DepotAktieRepo.LadeAnhandAktieID(inAktieID);

            if (depotAktie == null)
            {
                depotAktie = new DepotAktie { AktieID = inAktieID, DepotID = 1 };
            }
            //var AlteAnzahl = depotAktie.Anzahl;

            depotAktie.Anzahl +=inAnzahl;

            //depotAktie.BuyIn = (( depotAktie.BuyIn * AlteAnzahl) + ((inPreis * inAnzahl) + inFremdkosten.GetValueOrDefault(0))) / depotAktie.Anzahl;
            DepotAktieRepo.Speichern(depotAktie);
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
                DepotAktie.BuyIn = ((DepotAktie.BuyIn * AlteAnzahl) - (Order.Preis * Order.Anzahl) - Order.Fremdkostenzuschlag.GetValueOrDefault(0)) / DepotAktie.Anzahl;
                DepotAktieRepo.Speichern(DepotAktie);
            }
            OrderRepo.Entfernen(Order);
        }

        public void NeueAktieVerkauft(double inPreis, double? inFremdkosten, DateTime inDatum, int inAktieID, int inAnzahl, KaufTypes inKauftyp, OrderTypes inOrderTyp)
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
                DepotAktie.BuyIn = ((DepotAktie.BuyIn * AlteAnzahl) - (inPreis * inAnzahl) - inFremdkosten.GetValueOrDefault(0)) / DepotAktie.Anzahl;
                DepotAktieRepo.Speichern(DepotAktie);
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
            DepotAktieRepo.Speichern(DepotAktie);
           
            OrderRepo.Entfernen(Order);
        }
       
        public ObservableCollection<DepotAktie> LadeAlleVorhandeneImDepot()
        {
            return new DepotAktienRepository().LoadAll();
        }

        public void NeueDividendenErhalten(int inAktieID, int inDividendeID, DateTime inDatum, Double? inQuellensteuer, Double? inUmrechnungskurs, int inBestand)
        {
            var Erhaltenedividende = new DividendeErhalten { AktieID = inAktieID, DividendeID = inDividendeID, Bestand = inBestand, Datum = inDatum, Quellensteuer = inQuellensteuer, Umrechnungskurs = inUmrechnungskurs };
            NeueDividendenErhalten(Erhaltenedividende);
        }

        public void NeueDividendenErhalten(DividendeErhalten inDividendeErhalten)
        {
            var dividende = new DividendeRepository().LadeAnhandID(inDividendeErhalten.DividendeID);

            inDividendeErhalten.GesamtBrutto = (dividende.Betrag * inDividendeErhalten.Bestand);
            inDividendeErhalten.GesamtNetto = inDividendeErhalten.GesamtBrutto - inDividendeErhalten.Quellensteuer.GetValueOrDefault(0);

            if( (!dividende.Waehrung.Equals(Waehrungen.Euro)) && ( !dividende.BetragUmgerechnet.HasValue ))
            {
                dividende.BetragUmgerechnet = dividende.Betrag / inDividendeErhalten.Umrechnungskurs.GetValueOrDefault(1);
                new DividendeRepository().Update(dividende.Betrag, dividende.Datum, dividende.ID, dividende.Waehrung, dividende.BetragUmgerechnet);
            }

            new DividendeErhaltenRepository().Speichern(inDividendeErhalten);
        }
    }

    public class AktieSchonVorhandenException : Exception
    { }
}
