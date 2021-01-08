using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aktien.Data.Types;

namespace Aktien.Logic.Messages.DividendeMessages
{
    public class OpenDividendeStammdatenNeuMessage
    {
        public String Aktienname { get; set; }
        public int AktieID { get; set; }
        public State State { get; set; }
        public int? DividendeID { get; set; }
    }

}
