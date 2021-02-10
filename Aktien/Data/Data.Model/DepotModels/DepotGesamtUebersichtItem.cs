using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aktien.Data.Model.DepotModels
{
    public class DepotGesamtUebersichtItem
    {
        public int WertpapierID { get; set; }
        public int DepotWertpapierID { get; set; }
        public Double Anzahl { get; set; }
        public Double BuyIn { get; set; }
        public WertpapierTypes WertpapierTyp { get; set; }
        public String Bezeichnung { get; set; }
        public Double Gesamt { get { return Anzahl * BuyIn; } }
    }
}
