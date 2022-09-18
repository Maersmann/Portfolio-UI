using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.WertpapierModels
{
    public class SplitModel
    {
        public string WertpapierName { get; set; }
        public double NeueAnzahl { get; set; }
        public double NeuerBuyIn { get; set; }
        public double AlteAnzahl { get; set; }
        public double AlterBuyIn { get; set; }
        public int DepotWertpapierID { get; set; }
        public DateTime? Datum { get; set; }
    }
}
