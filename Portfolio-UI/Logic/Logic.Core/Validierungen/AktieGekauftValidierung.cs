﻿using Aktien.Data.Types.WertpapierTypes;
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
        public bool ValidateBetrag(Double? betrag, KaufTypes kauftyp,  out ICollection<string> validatonErrors)
        {
            validatonErrors = new List<String>();

            if (!betrag.HasValue)
                validatonErrors.Add("Kein Betrag hinterlegt");

            if ((betrag == 0) &&((!kauftyp.Equals(KaufTypes.SpinOff))&&(!kauftyp.Equals(KaufTypes.Ausbuchung) && (!kauftyp.Equals(KaufTypes.Einbuchung)))))
                validatonErrors.Add("Der Betrag darf nicht 0 sein");

            return validatonErrors.Count == 0;
        }

    }
}
