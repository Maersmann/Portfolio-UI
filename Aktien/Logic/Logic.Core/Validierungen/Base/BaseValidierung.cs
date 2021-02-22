using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.Validierung.Base
{
    public class BaseValidierung
    {
        public bool ValidateBetrag(Double? betrag, out ICollection<string> validatonErrors)
        {
            validatonErrors = new List<String>();

            if (!betrag.HasValue)
                validatonErrors.Add("Kein Betrag hinterlegt sein");

            if ((betrag == 0))
                validatonErrors.Add("Der Betrag darf nicht 0 sein");

            return validatonErrors.Count == 0;
        }

        public bool ValidateDatum(DateTime? date, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (!date.HasValue)
                validationErrors.Add("Kein Datum hinterlegt");

            return validationErrors.Count == 0;
        }
    }
}
