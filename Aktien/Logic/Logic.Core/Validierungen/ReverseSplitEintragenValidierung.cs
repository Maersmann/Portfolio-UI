using Aktien.Logic.Core.Validierung.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.Validierungen
{
    public class ReverseSplitEintragenValidierung : BaseValidierung
    {
        public bool ValidateAktie(String name,  out ICollection<string> validationErrors)
        {
            validationErrors = new List<String>();

            if (name.Length == 0)
                validationErrors.Add("Keine Aktie ausgewält");

            return validationErrors.Count == 0;
        }
    }
}
