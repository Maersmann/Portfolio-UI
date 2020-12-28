using Logic.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Core.Validierung
{
    public class DividendeStammdatenValidierung : IValidierung
    {
        public bool ValidateDatum(DateTime? inDate, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (!inDate.HasValue)
                validationErrors.Add("Kein Datum hinterlegt");

            return validationErrors.Count == 0;
        }

        public bool ValidateBetrag(Double? inBetrag, out ICollection<string> validatonErrors  )
        {
            validatonErrors = new List<String>();

            if (!inBetrag.HasValue)
                validatonErrors.Add("Es muss ein Betrag hinterlegt sein");

            if (inBetrag == 0)
                validatonErrors.Add("Der Betrag darf nicht 0 sein");

            return validatonErrors.Count == 0;
        }
    }
}
