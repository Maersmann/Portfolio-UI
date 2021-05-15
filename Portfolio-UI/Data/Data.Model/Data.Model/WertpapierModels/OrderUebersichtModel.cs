using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.WertpapierModels
{
    public class OrderUebersichtModel
    {
        public int ID { get; set; }
        public DateTime Orderdatum { get; set; }
        public Double Anzahl { get; set; }
        public Double Preis { get; set; }
        public Double? Fremdkostenzuschlag { get; set; }
        public BuySell BuySell { get; set; }
    }
}
