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
        public void NeuBerechnen(int inWertpapierID)
        {
            DepotWertpapier dw = new DepotWertpapier { DepotID = 1, WertpapierID = inWertpapierID, BuyIn = 0, Anzahl = 0 };
            IList<OrderHistory> orders = new OrderHistoryRepository().LadeAlleByWertpapierID(inWertpapierID);

            orders.OrderBy(o => o.Orderdatum);

            orders.ToList().ForEach(o => 
            {
                if (o.BuySell == BuySell.Buy)
                {
                    var AlteAnzahl = dw.Anzahl;
                    dw.Anzahl += o.Anzahl;
                    dw.BuyIn = new KaufBerechnungen().BuyInAktieGekauft(dw.BuyIn, AlteAnzahl, dw.Anzahl, o.Preis, o.Anzahl, o.Fremdkostenzuschlag);
                }             
                else
                    dw.Anzahl -= o.Anzahl;
            });

            new DepotWertpapierRepository().Speichern(dw);
        }
    }
}
