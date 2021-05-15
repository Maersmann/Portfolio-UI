using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DepotModels
{
    public class BuyOrderModel //OrderHistory
    {
        public int WertpapierID { get; set; }
        public KaufTypes KaufartTyp { get; set; }
        public OrderTypes OrderartTyp { get; set; }
        public DateTime Orderdatum { get; set; }
        public double Preis { get; set; }
        public double? Fremdkostenzuschlag { get; set; }
        public double Anzahl { get; set; }
        public double? Gesamtbetrag { get; set; }
    }
}
