using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DividendeModels
{
    public class DividendeErhaltenUebersichtModel
    {
        public int ID { get; set; }
        public double GesamtNetto { get; set; }
        public double GesamtBrutto { get; set; }
        public double? Quellensteuer { get; set; }
        public DateTime? Exdatum { get; set; }
        public DateTime Zahldatum { get; set; }
        public Waehrungen Waehrung { get; set; }
        public double Bestand { get; set; }
    }
}
