using Aktien.Data.Types.WertpapierTypes;
using Data.Model.SteuerModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DepotModels
{
    public class BuyOrderModel
    {
        public int WertpapierID { get; set; }
        public KaufTypes KaufartTyp { get; set; }
        public OrderTypes OrderartTyp { get; set; }
        public DateTime Orderdatum { get; set; }
        public double Preis { get; set; }
        public double? Fremdkostenzuschlag { get; set; }
        public double Anzahl { get; set; }
        public int? SteuergruppeID { get; set; }
        public Double Gesamt { get; set; }
        public Double Bemessungsgrundlage { get; set; }
        public SteuergruppeModel Steuer { get; set; }
    }
}
