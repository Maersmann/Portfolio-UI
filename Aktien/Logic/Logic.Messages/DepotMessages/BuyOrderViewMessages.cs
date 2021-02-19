using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.DepotMessages
{
    public class OpenAktieGekauftViewMessage
    {
        public int WertpapierID { get; set; }
        public BuySell BuySell { get; set; }
        public WertpapierTypes WertpapierTypes { get; set; }
    }


}
