using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.Aktie
{

    public class OpenAktieStammdatenMessage: BaseStammdatenMessage
    {
        public int WertpapierID { get; set; }
        
    }

    public class DeleteAktieErfolgreichMessage
    {

    }
}
