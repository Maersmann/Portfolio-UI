using Aktien.Logic.Core.Interfaces;
using Aktien.Logic.Core.Validierung.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.Validierung
{
    public class WertpapierStammdatenValidierung : BaseValidierung
    {
        public bool ValidateName(String name, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (name.Length > 250)
                validationErrors.Add("Der Name ist zu lang");

            if (name.Length == 0)
                validationErrors.Add("Der Name darf nicht leer sein");

            return validationErrors.Count == 0;
        }

        public bool ValidateISIN(String isin, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (isin.Length > 250)
                validationErrors.Add("Die ISIN ist zu lang");

            if (isin.Length == 0)
                validationErrors.Add("Die ISIN darf nicht leer sein");

            return validationErrors.Count == 0;
        }
    }
}
