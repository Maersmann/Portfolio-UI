using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DividendeModels
{
    public class DividendeModel
    {
        public int ID { get; set; }
        public int WertpapierID { get; set; }
        public DateTime? Exdatum { get; set; }
        public Waehrungen Waehrung { get; set; }
        public double? BetragUmgerechnet { get; set; }
        public double Betrag { get; set; }
        public DateTime Zahldatum { get; set; }
        public DividendenRundungTypes RundungArt { get; set; }
        public Boolean Sonderdividende { get; set; }
    }
}
