using Aktien.Data.Types;
using Aktien.Logic.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.Validierung
{
    public class AktieGekauftValidierung : IValidierung
    {
        public bool ValidateAnzahl(Double? inAnzahl, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (!inAnzahl.HasValue)
                validationErrors.Add("Keine Anzahl hinterlegt");

            if (inAnzahl == 0)
                validationErrors.Add("Die Anzahl darf nicht 0 sein");

            if (inAnzahl < 0)
                validationErrors.Add("Die Anzahl zu niedrig");

            return validationErrors.Count == 0;
        }
        public bool ValidateBetrag(Double? inBetrag, KaufTypes inKaufTyp,  out ICollection<string> validatonErrors)
        {
            validatonErrors = new List<String>();

            if (!inBetrag.HasValue)
                validatonErrors.Add("Kein Betrag hinterlegt sein");

            if ((inBetrag == 0) && (!inKaufTyp.Equals(KaufTypes.SpinOff)) )
                validatonErrors.Add("Der Betrag darf nicht 0 sein");

            return validatonErrors.Count == 0;
        }

        public bool ValidateDatum(DateTime? inDate, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (!inDate.HasValue)
                validationErrors.Add("Kein Datum hinterlegt");

            return validationErrors.Count == 0;
        }

    }
}
