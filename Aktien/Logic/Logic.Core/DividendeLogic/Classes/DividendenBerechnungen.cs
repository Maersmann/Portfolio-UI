using Aktien.Data.Types.DividendenTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.DividendeLogic.Classes
{
    public class DividendenBerechnungen
    {
        public Double GesamtBrutto(double betrag, Double bestand)
        {
            if (bestand < 0) return 0;
            return Math.Round((betrag * bestand),2, MidpointRounding.AwayFromZero);
        }

        public Double GesamtNetto(double gesamtBrutto, Double? quellensteuer)
        {
            return Math.Round(gesamtBrutto - quellensteuer.GetValueOrDefault(0),2, MidpointRounding.AwayFromZero);
        }

        public Double BetragUmgerechnet( Double betrag, Double? umrechnungskurs, bool mitRunden, DividendenRundungTypes typ )
        {
            betrag /= umrechnungskurs.GetValueOrDefault(1);
            if (mitRunden)
            {
                switch (typ)
                {
                    case DividendenRundungTypes.Normal:
                        betrag = Math.Round(betrag, 2, MidpointRounding.AwayFromZero);
                        break;
                    case DividendenRundungTypes.Up:
                        betrag = RoundUp(betrag, 2);
                        break;
                    case DividendenRundungTypes.Down:
                        betrag = RoundDown(betrag, 2);
                        break;
                }
               
            }

            return betrag;
        }


        private static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }

        private static double RoundDown(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Floor(input * multiplier) / multiplier;
        }
    }
}
