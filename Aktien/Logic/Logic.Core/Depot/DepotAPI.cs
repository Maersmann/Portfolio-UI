using Aktien.Data.Infrastructure.Depots.Repository;
using Aktien.Data.Model.DepotModels;
using System;
using System.Collections.Generic;
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
    }

    public class AktieSchonVorhandenException : Exception
    { }
}
