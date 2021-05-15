using Aktien.Data.Types.DepotTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DepotModels
{
    public class EinnahmeModel
    {
        public DateTime Datum { get; set; }
        public double Betrag { get; set; }
        public string Beschreibung { get; set; }
        public EinnahmeArtTypes Art { get; set; }
        public int DepotID { get; set; }
        public int ID { get; set; }
    }
}
