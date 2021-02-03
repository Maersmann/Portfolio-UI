using Aktien.Logic.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.Validierung
{
    public class AktieStammdatenValidierung : IValidierung
    {
        public bool ValidateName(String inName, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (inName.Length > 250)
                validationErrors.Add("Der Name ist zu lang");

            if (inName.Length == 0)
                validationErrors.Add("Der Name darf nicht leer sein");

            return validationErrors.Count == 0;
        }

        public bool ValidateISIN(String inISIN, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (inISIN.Length > 250)
                validationErrors.Add("Die ISIN ist zu lang");

            if (inISIN.Length == 0)
                validationErrors.Add("Die ISIN darf nicht leer sein");

            return validationErrors.Count == 0;
        }
    }
}
