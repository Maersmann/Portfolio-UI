using Data.Model.SteuerModels;
using Data.Types.SteuerTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.SteuernLogic
{
    public class SteuerBerechnen
    {
        public double SteuernVorZwischensumme(IList<SteuerModel> steuern)
        {
            Double ret = 0;

            steuern.ToList().ForEach(s =>
            {
                if (s.Steuerart.BerechnungZwischensumme.Equals(SteuerberechnungZwischensumme.vorZwischensumme))
                {
                    if (s.Optimierung)
                        ret += s.Betrag;
                    else
                        ret -= s.Betrag;
                }
            });

            return ret;
        }
        public double SteuernNachZwischensumme(IList<SteuerModel> steuern)
        {
            Double ret = 0;

            steuern.ToList().ForEach(s =>
            {
                if (s.Steuerart.BerechnungZwischensumme.Equals(SteuerberechnungZwischensumme.nachZischensumme))
                {
                    if (s.Optimierung)
                        ret += s.Betrag;
                    else
                        ret -= s.Betrag;
                }
            });

            return ret;
        }

        public double SteuerGesamt(IList<SteuerModel> steuern)
        {
            Double ret = 0;

            steuern.ToList().ForEach(s =>
            {
                if (s.Optimierung)
                    ret += s.Betrag;
                else
                    ret -= s.Betrag;
            });

            return ret;
        }
    }
}
