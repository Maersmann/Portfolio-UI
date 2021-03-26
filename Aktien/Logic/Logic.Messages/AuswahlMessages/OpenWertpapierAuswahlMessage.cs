using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.AuswahlMessages
{
    public class OpenWertpapierAuswahlMessage
    {
        public Action<bool, int> Callback { get; private set; }

        public OpenWertpapierAuswahlMessage(Action<bool, int> callback)
        {
            Callback = callback;
        }
    }
}
