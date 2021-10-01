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
        public bool ValidateBestand(double bestand, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (bestand == 0)
                validationErrors.Add("Kein Bestand hinterlegt");

            if (bestand < 0)
                validationErrors.Add("Der Bestand ist zu niedrig");

            return validationErrors.Count == 0;
        }

        public bool ValidateDividende(int id, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (id == -1)
                validationErrors.Add("Keine Dividende ausgewählt");

            return validationErrors.Count == 0;
        }
    }
}
