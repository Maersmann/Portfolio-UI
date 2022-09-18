using Data.Types.SparplanTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.SparplanModels
{
    public class SparplanModel
    {
        public int ID { get; set; }
        public int WertpapierID { get; set; }
        public string WertpapierName { get; set; }
        public string Betrag { get; set; }
        public SparplanIntervall Intervall{get;set;}
        public SparplanStartDatum StartDatum { get; set; }
    }
}
