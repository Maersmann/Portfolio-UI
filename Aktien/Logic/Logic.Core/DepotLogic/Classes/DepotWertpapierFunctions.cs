using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.DepotLogic.Classes
{
    public class DepotWertpapierFunctions
    {
        public void NeuBerechnen(DepotWertpapier depotWertpapier)
        {
            depotWertpapier.BuyIn = 0;
            depotWertpapier.Anzahl = 0;
            IList<OrderHistory> orders = new OrderHistoryRepository().LadeAlleByWertpapierID(depotWertpapier.WertpapierID);

            orders.OrderBy(o => o.Orderdatum);

            orders.ToList().ForEach(o =>
            {
                if (o.BuySell == BuySell.Buy)
                {
                    var AlteAnzahl = depotWertpapier.Anzahl;
                    depotWertpapier.Anzahl += o.Anzahl;
                    depotWertpapier.BuyIn = new KaufBerechnungen().BuyInAktieGekauft(depotWertpapier.BuyIn, AlteAnzahl, depotWertpapier.Anzahl, o.Preis, o.Anzahl, o.Fremdkostenzuschlag, o.OrderartTyp);
                }
                else
                    depotWertpapier.Anzahl -= o.Anzahl;
            });
        }
    }
}
