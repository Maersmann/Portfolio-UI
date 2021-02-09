using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.DerivateMessages
{
    public class OpenDerivateStammdatenMessage
    {
        public int WertpapierID { get; set; }
        public State State { get; set; }
    }
}
