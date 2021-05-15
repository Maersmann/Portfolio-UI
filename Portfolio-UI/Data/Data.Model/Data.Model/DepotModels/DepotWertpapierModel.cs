using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DepotModels
{
    public class DepotWertpapierModel
    {
        public int ID { get; set; }
        public Double Anzahl { get; set; }
        public Double BuyIn { get; set; }
        public int WertpapierID { get; set; }
        public int DepotID { get; set; }
        public string Name { get; set; }
    }
}
