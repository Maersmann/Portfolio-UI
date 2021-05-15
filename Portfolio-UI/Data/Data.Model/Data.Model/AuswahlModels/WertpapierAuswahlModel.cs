using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswahlModels
{
    public class WertpapierAuswahlModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ISIN { get; set; }
        public WertpapierTypes WertpapierTyp { get; set; }
    }
}
