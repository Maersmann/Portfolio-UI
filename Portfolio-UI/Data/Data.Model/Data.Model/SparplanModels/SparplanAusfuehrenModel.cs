using Data.Model.WertpapierModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.SparplanModels
{
    public class SparplanAusfuehrenModel
    {
        public int ID { get; set; }
        public string WertpapierName { get; set; }
        public DateTime? NaechsteAusfuehrung { get; set; }
        public string Betrag { get; set; }
        public string Anzahl { get; set; }
        public double BuyIn { get; set; }
    }
}
