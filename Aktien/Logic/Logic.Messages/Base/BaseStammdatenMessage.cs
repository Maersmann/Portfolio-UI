using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.Base
{
    public class BaseStammdatenMessage
    {
        public ViewType ViewType { get; set; }
        public State State { get; set; }
        public int? ID { get; set; }
    }
}
