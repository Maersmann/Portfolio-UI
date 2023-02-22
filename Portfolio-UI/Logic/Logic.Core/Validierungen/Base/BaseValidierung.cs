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
                validatonErrors.Add("Kein Betrag hinterlegt");

            if ((betrag == 0))
                validatonErrors.Add("Der Betrag darf nicht 0 sein");

            return validatonErrors.Count == 0;
        }

        public bool ValidateBetrag(string betrag, out ICollection<string> validatonErrors, bool erlaubeNull = false)
        {
            validatonErrors = new List<string>();

            if (!double.TryParse(betrag, out double Betrag))
            {
                validatonErrors.Add("Kein Betrag hinterlegt");
                return false;
            }

            if (!erlaubeNull && Betrag == 0)
            {
                validatonErrors.Add("Der Betrag darf nicht 0 sein");
            }

            return validatonErrors.Count == 0;
        }




        public bool ValidateDatum(DateTime? date, out ICollection<string> validationErrors)
        {
            validationErrors = new List<string>();

            if (date.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue))
                validationErrors.Add("Kein Datum hinterlegt");

            return validationErrors.Count == 0;
        }

        public bool ValidateAnzahl(double? anzahl, out ICollection<string> validationErrors)
        {
            validationErrors = new List<string>();

            if (!anzahl.HasValue)
            {
                validationErrors.Add("Keine Anzahl hinterlegt");
            }

            if (anzahl == 0)
            {
                validationErrors.Add("Die Anzahl darf nicht 0 sein");
            }

            if (anzahl < 0)
            {
                validationErrors.Add("Die Anzahl zu niedrig");
            }

            return validationErrors.Count == 0;
        }

        public bool ValidateAnzahl(string anzahl, out ICollection<string> validationErrors, bool erlaubeNull)
        {
            validationErrors = new List<string>();

            if (!double.TryParse(anzahl, out double Anzahl))
            {
                validationErrors.Add("Keine Anzahl hinterlegt");
                return false;
            }

            if (!erlaubeNull && Anzahl == 0)
            {
                validationErrors.Add("Die Anzahl darf nicht 0 sein");
            }

            if (Anzahl < 0)
            {
                validationErrors.Add("Die Anzahl zu niedrig");
            }

            return validationErrors.Count == 0;
        }

        public bool ValidateString(string name, string fieldname, out ICollection<string> validationErrors)
        {
            validationErrors = new List<string>();

            if (name.Length > 250)
                validationErrors.Add( fieldname+" ist zu lang");

            if (name.Length == 0)
                validationErrors.Add( fieldname + " darf nicht leer sein");

            return validationErrors.Count == 0;
        }

        public bool ValidateZahl(int? zahl, out ICollection<string> validationErrors)
        {
            validationErrors = new List<string>();

            if (!zahl.HasValue)
                validationErrors.Add("Keine Zahl hinterlegt");

            if (zahl == 0)
                validationErrors.Add("Die Zahl darf nicht 0 sein");

            if (zahl < 0)
                validationErrors.Add("Die Zahl zu niedrig");

            return validationErrors.Count == 0;
        }

        public bool ValidateZahl(string zahl, out ICollection<string> validationErrors)
        {
            validationErrors = new List<string>();

            if (!Double.TryParse(zahl, out double Zahl))
            {
                validationErrors.Add("Keine Zahl hinterlegt");
                return false;
            }

            if (Zahl == 0)
                validationErrors.Add("Die Zahl darf nicht 0 sein");

            if (Zahl < 0)
                validationErrors.Add("Die Zahl zu niedrig");

            return validationErrors.Count == 0;
        }

        public bool ValidateZahl(string betrag, out ICollection<string> validatonErrors, bool erlaubeNull = false)
        {
            validatonErrors = new List<string>();

            if (!double.TryParse(betrag, out double Betrag))
            {
                validatonErrors.Add("Keine Zahl hinterlegt");
                return false;
            }

            if (!erlaubeNull && Betrag == 0)
            {
                validatonErrors.Add("Die Zahl darf nicht 0 sein");
            }

            return validatonErrors.Count == 0;
        }
    }
}
