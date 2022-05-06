using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels.DividendeModels
{
    public class DividendeGesamtentwicklungMonatlichSummiertModel
    {
        public DateTime Datum { get; set; }
        public double Netto { get; set; }
        public double Brutto { get; set; }
    }
}
