using Data.Model.WertpapierModels;
using Data.Types.SparplanTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.SparplanModels
{
    public class SparplanUebersichtModel
    {
        public int ID { get; set; }
        public WertpapierModel Wertpapier { get; set; }
        public DateTime? NaechsteAusfuehrung { get; set; }
        public SparplanIntervall Intervall { get; set; }
        public SparplanStartDatum StartDatum { get; set; }
        public SparplanStatus Status { get; set; }
        public double Betrag { get; set; }


        public bool CanBeenden => Status.Equals(SparplanStatus.aktiv);

    }
}
