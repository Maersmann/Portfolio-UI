using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswahlModels
{
    public class DividendenAuswahlModel // Dividende
    {
        public int ID { get; set; }
        public double Betrag { get; set; }
        public DateTime Zahldatum { get; set; }
        public Waehrungen Waehrung { get; set; }
    }
}
