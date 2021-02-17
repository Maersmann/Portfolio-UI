using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.WertpapierMessages
{
    public class LoadWertpapierOrderMessage
    {
        public int WertpapierID { get; set; }
        public WertpapierTypes WertpapierTyp { get; set; }
    }
}
