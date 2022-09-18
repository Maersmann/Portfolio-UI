using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Messages.DepotMessages
{
    public class OpenErhalteneDividendeEintragenMessage
    {
        public int WertpapierID { get; set; }
        public string WertpapierName { get; set; }
    }
}
