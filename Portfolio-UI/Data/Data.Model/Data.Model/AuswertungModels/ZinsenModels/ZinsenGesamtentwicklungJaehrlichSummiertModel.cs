using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels.ZinsenModels
{
    public class ZinsenGesamtentwicklungJaehrlichSummiertModel
    {
        public int Jahr { get; set; }
        public Decimal Gesamt { get; set; }
        public Decimal Erhalten { get; set; }
    }
}
