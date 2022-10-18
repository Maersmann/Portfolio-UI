using Data.Types.SparplanTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.SparplanModels
{
    public class SparplanHistoryUebersichtModel
    {
        public DateTime AusfuehrungAm { get; set; }
        public double Anzahl { get; set; }
        public double Preis { get; set; }
        public double Gesamt { get; set; }
        public SparplanHistoryArt Art { get; set; }
    }
}
