using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.OptionenModels
{
    public class WertpapierBuyInModel
    {
        public string WertpapierName { get; set; }
        public double AlterBuyIn { get; set; }
        public double NeuerBuyIn { get; set; }
        public int DepotWertpapierID { get; set; }
        public int WertpapierID { get; set; }
        public int DepotID { get; set; }
        public Double Anzahl { get; set; }
    }
}
