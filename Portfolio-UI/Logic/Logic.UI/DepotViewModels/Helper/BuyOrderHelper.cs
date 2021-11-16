using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.UI.DepotViewModels.Helper
{
    public class BuyOrderHelper
    {

        public static IEnumerable<KaufTypes> GetKaufTypes(BuySell buySell)
        {
            if (buySell.Equals(BuySell.Buy))
                return new List<KaufTypes> { KaufTypes.Kauf, KaufTypes.Einbuchung, KaufTypes.Kapitalerhoehung, KaufTypes.SpinOff };
            else
                return new List<KaufTypes> { KaufTypes.Kauf, KaufTypes.Ausbuchung };
        }

        public static IEnumerable<OrderTypes> GetOrderTypes(BuySell buySell)
        {
            if (buySell.Equals(BuySell.Buy))
                return new List<OrderTypes> { OrderTypes.Normal, OrderTypes.Limit, OrderTypes.Stop, OrderTypes.Sparplan };
            else
                return new List<OrderTypes> { OrderTypes.Normal, OrderTypes.Limit, OrderTypes.Stop };
        }

    }
}
