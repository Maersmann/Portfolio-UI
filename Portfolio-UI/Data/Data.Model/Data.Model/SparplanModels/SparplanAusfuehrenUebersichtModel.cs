using Data.Model.WertpapierModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.SparplanModels
{
    public class SparplanAusfuehrenUebersichtModel
    {
        public int ID { get; set; }
        public WertpapierModel Wertpapier { get; set; }
        public DateTime NaechsteAusfuehrung { get; set; }
        public double Betrag { get; set; }

        public bool CanAusfuehren => NaechsteAusfuehrung <= DateTime.Now;

    }
}
