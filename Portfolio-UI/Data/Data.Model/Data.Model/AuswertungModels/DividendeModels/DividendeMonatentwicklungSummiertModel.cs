using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels.DividendeModels
{
    public class DividendeMonatentwicklungSummiertModel
    {
        public int Monat { get; set; }
        public int Jahr { get; set; }
        public double Netto { get; set; }
        public double Brutto { get; set; }
    }
}
