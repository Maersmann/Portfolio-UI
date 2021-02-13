using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.DividendeLogic.Classes
{
    public class DividendenBerechnungen
    {
        public Double GesamtBrutto(double inBetrag, Double inBestand)
        {
            return (inBetrag * inBestand);
        }

        public Double GesamtNetto(double inGesamtBrutto, Double? inQuellensteuer)
        {
            return inGesamtBrutto - inQuellensteuer.GetValueOrDefault(0);
        }

        public Double BetragUmgerechnet( Double inBetrag, Double? inUmrechnungskurs )
        {
            return inBetrag / inUmrechnungskurs.GetValueOrDefault(1);
        }
    }
}
