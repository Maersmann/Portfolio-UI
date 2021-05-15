using Aktien.Data.Types.DepotTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DepotModels
{
    public class AusgabeModel
    {
        public int DepotID { get; set; }
        public AusgabenArtTypes Art { get; set; }
        public string Beschreibung { get; set; }
        public double Betrag { get; set; }
        public DateTime Datum { get; set; }
        public int ID { get; set; }
    }
}
