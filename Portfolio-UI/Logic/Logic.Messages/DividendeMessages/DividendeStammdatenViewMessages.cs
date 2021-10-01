using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using Base.Logic.Messages;

namespace Aktien.Logic.Messages.DividendeMessages
{
    public class OpenDividendeStammdatenMessage<T> : BaseStammdatenMessage<T>
    {
        public String Aktienname { get; set; }
        public int WertpapierID { get; set; }
        public int? DividendeID { get; set; }
    }

}
