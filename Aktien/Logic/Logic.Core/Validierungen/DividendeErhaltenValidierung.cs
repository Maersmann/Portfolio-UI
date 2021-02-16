using Aktien.Logic.Core.Validierung.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.Validierung
{
    public class DividendeErhaltenValidierung : BaseValidierung
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

        public bool ValidateDividende(int inID, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (inID == -1)
                validationErrors.Add("Keine Dividende ausgewählt");

            return validationErrors.Count == 0;
        }
    }
}
