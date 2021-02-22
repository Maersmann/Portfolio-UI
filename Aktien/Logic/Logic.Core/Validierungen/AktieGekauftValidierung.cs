using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.Interfaces;
using Aktien.Logic.Core.Validierung.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.Validierung
{
    public class AktieGekauftValidierung : BaseValidierung
    {
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
        public bool ValidateBetrag(Double? betrag, KaufTypes kauftyp,  out ICollection<string> validatonErrors)
        {
            validatonErrors = new List<String>();

            if (!betrag.HasValue)
                validatonErrors.Add("Kein Betrag hinterlegt");

            if ((betrag == 0) && (!kauftyp.Equals(KaufTypes.SpinOff)) )
                validatonErrors.Add("Der Betrag darf nicht 0 sein");

            return validatonErrors.Count == 0;
        }

    }
}
