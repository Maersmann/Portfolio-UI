using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.AuswahlMessages
{
    public class OpenDividendenAuswahlMessage
    {
        public int WertpapierID { get; set; }
        public Action<bool, int, Double, DateTime> Callback { get; private set; }

        public OpenDividendenAuswahlMessage(Action<bool, int, Double, DateTime> callback, int wertpapierid)
        {
            Callback = callback;
            WertpapierID = wertpapierid;
        }
    }
}
