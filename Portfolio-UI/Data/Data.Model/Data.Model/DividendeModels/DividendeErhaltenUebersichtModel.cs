using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DividendeModels
{
    public class DividendeErhaltenUebersichtModel: DividendeErhaltenModel
    {
        public double? Steuern { get; set; }
        public DateTime? Exdatum { get; set; }
        public DateTime Zahldatum { get; set; }
        public Waehrungen Waehrung { get; set; }
    }
}
