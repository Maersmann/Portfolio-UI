using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.Base
{

    public class StammdatenGespeichertMessage
    {
        public bool Erfolgreich { get; set; }
        public string Message { get; set; }
    }

    public class ExceptionMessage
    {
        public string Message { get; set; }
    }

    public class InformationMessage
    {
        public string Message { get; set; }
    }
}
