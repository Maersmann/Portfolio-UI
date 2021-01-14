using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.DepotModels;
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
            orderHistoryRepo.Speichern(inPreis, inFremdkosten, inDatum, inAktieID, inAnzahl, inKauftyp, inOrderTyp);

            var DepotAktieRepo = new DepotAktienRepository();
            var depotAktie = DepotAktieRepo.LadeAnhandAktieID(inAktieID);

            if (depotAktie == null)
            {
                depotAktie = new DepotAktie { AktieID = inAktieID, DepotID = 1 };
            }
            var AlteAnzahl = depotAktie.Anzahl;

            depotAktie.Anzahl +=inAnzahl;

            depotAktie.BuyIn = (( depotAktie.BuyIn * AlteAnzahl) + ((inPreis * inAnzahl) + inFremdkosten.GetValueOrDefault(0))) / depotAktie.Anzahl;
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

        public ObservableCollection<DepotAktie> LadeAlleVorhandeneImDepot()
        {
            return new DepotAktienRepository().LoadAll();
        }
    }

    public class AktieSchonVorhandenException : Exception
    { }
}
