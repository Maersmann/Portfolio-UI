using Data.DTO.WertpapierDTOs;
using Data.Types.SparplanTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO.SparplanDTOs
{
    public class SparplanDTO
    {
        public int ID { get; set; }
        public int WertpapierID { get; set; }
        public double Betrag { get; set; }
        public WertpapierDTO Wertpapier { get; set; }
        public SparplanIntervall Intervall { get; set; }
        public SparplanStartDatum StartDatum { get; set; }
    }
}
