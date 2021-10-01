using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using Base.Logic.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.DividendeMessages
{
    public class OpenErhaltendeDividendeStammdatenMessage<T> : BaseStammdatenMessage<T>
    {
        public int WertpapierID { get; set; }
    }
}
