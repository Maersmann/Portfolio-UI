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

            if (date.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue))
                validationErrors.Add("Kein Datum hinterlegt");

            return validationErrors.Count == 0;
        }

        public bool ValidateAnzahl(Double? anzahl, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (!anzahl.HasValue)
                validationErrors.Add("Keine Anzahl hinterlegt");

            if (anzahl == 0)
                validationErrors.Add("Die Anzahl darf nicht 0 sein");

            if (anzahl < 0)
                validationErrors.Add("Die Anzahl zu niedrig");

            return validationErrors.Count == 0;
        }

        public bool ValidateString(String name, String fieldname, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (name.Length > 250)
                validationErrors.Add( fieldname+" ist zu lang");

            if (name.Length == 0)
                validationErrors.Add( fieldname + " darf nicht leer sein");

            return validationErrors.Count == 0;
        }

        public bool ValidateZahl(int? zahl, out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (!zahl.HasValue)
                validationErrors.Add("Keine Zahl hinterlegt");

            if (zahl == 0)
                validationErrors.Add("Die Zahl darf nicht 0 sein");

            if (zahl < 0)
                validationErrors.Add("Die Zahl zu niedrig");

            return validationErrors.Count == 0;
        }
    }
}
