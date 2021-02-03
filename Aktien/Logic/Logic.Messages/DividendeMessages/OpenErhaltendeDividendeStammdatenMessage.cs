using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.DividendeMessages
{
    public class OpenErhaltendeDividendeStammdatenMessage
    {
        public int AktieID { get; set; }
        public State State { get; set; }
        public int? ID { get; set; }
    }
}
