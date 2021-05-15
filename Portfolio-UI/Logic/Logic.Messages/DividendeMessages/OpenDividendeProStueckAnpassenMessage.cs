using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.DividendeMessages
{
    public class OpenDividendeProStueckAnpassenMessage
    {
        public int DividendeID { get; set; }
        public double Umrechnungskurs { get; set; }
    }
}
