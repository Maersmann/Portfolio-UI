using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DividendeModels
{
    public class DividendeProStueckAnpassenModel : DividendeProStueckAnpassenDTOModel
    {
        public DateTime Zahldatum { get; set; }
        public double Betrag { get; set; }
        public Waehrungen Waehrung { get; set; }

    }

    public class DividendeProStueckAnpassenDTOModel
    {
        public int DividendeID { get; set; }
        public DividendenRundungTypes Rundungart { get; set; }
        public Double Umrechnungskurs { get; set; }
        public DividendeProStueckAnpassenDTOModel()
        {
            Umrechnungskurs = 0;
            Rundungart = DividendenRundungTypes.Normal;
        }
    }
}
