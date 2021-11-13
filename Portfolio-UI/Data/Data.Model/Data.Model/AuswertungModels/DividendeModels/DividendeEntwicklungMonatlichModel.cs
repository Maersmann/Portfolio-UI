using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels
{
    public class DividendeEntwicklungMonatlichModel
    {
        public DateTime Datum { get; set; }
        public double Netto { get; set; }
        public double Brutto { get; set; }
    }
}
