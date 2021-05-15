using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DepotModels
{
    public class DepotGesamtUebersichtModel
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
