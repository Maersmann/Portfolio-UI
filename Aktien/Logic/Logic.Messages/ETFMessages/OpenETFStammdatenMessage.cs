using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.ETFMessages
{
    public class OpenETFStammdatenMessage : BaseStammdatenMessage
    {
        public int WertpapierID { get; set; }
    }
}
