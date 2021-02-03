using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.Validierung
{
    public class DividendeErhaltenValidierung
    {
        public bool ValidateBestand(Double? inBestand, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (!inBestand.HasValue || (inBestand == 0))
                validationErrors.Add("Kein Bestand hinterlegt");

            if (inBestand < 0)
                validationErrors.Add("Der Bestand ist zu niedrig");

            return validationErrors.Count == 0;
        }

        public bool ValidateBetrag(Double? inBetrag, out ICollection<string> validatonErrors)
        {
            validatonErrors = new List<String>();

            if (!inBetrag.HasValue)
                validatonErrors.Add("Kein Betrag hinterlegt sein");

            if (inBetrag == 0)
                validatonErrors.Add("Der Betrag darf nicht 0 sein");

            return validatonErrors.Count == 0;
        }

        public bool ValidateDatum(DateTime? inDate, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (!inDate.HasValue || ( inDate.Value.Equals(DateTime.MinValue) ))
                validationErrors.Add("Kein Datum hinterlegt");

            return validationErrors.Count == 0;
        }

        public bool ValidateDividende(int inID, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (inID == -1)
                validationErrors.Add("Keine Dividende ausgewählt");

            return validationErrors.Count == 0;
        }
    }
}
