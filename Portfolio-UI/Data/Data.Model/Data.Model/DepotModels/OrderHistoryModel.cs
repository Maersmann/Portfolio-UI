using Aktien.Data.Types.WertpapierTypes;
using Data.Model.SteuerModels;
using Data.Model.WertpapierModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DepotModels
{
    public class OrderHistoryModel
    {
        public int ID { get; set; }
        public double Preis { get; set; }
        public double? Fremdkostenzuschlag { get; set; }
        public DateTime Orderdatum { get; set; }
        public int WertpapierID { get; set; }
        public double Anzahl { get; set; }
        public KaufTypes KaufartTyp { get; set; }
        public OrderTypes OrderartTyp { get; set; }
        public double Gesamt { get; set; }
        public double Bemessungsgrundlage { get; set; }
        public int? SteuergruppeID { get; set; }
        public BuySell BuySell { get; set; }
        public SteuergruppeModel Steuer { get; set; }
        public WertpapierModel Wertpapier { get; set; }
    }
}
