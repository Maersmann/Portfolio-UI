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
            return Math.Round((inBetrag * inBestand),2, MidpointRounding.AwayFromZero);
        }

        public Double GesamtNetto(double inGesamtBrutto, Double? inQuellensteuer)
        {
            return Math.Round(inGesamtBrutto - inQuellensteuer.GetValueOrDefault(0),2, MidpointRounding.AwayFromZero);
        }

        public Double BetragUmgerechnet( Double inBetrag, Double? inUmrechnungskurs )
        {
            return Math.Round(inBetrag / inUmrechnungskurs.GetValueOrDefault(1),2, MidpointRounding.AwayFromZero);
        }
    }
}
