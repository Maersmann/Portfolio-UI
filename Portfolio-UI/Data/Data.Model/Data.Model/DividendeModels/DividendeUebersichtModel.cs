using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DividendeModels
{
    public class DividendeUebersichtModel : DividendeModel
    {
        public Double Eurobetrag { get { var ret = Betrag; if (BetragUmgerechnet.HasValue) ret = BetragUmgerechnet.Value; return ret; } }
        public static Waehrungen EuroWaehrung { get { return Waehrungen.Euro; } }
    }
}
