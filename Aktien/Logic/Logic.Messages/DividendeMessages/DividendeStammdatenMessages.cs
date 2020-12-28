using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Messages.DividendeMessages
{
    public class OpenDividendeStammdatenNeuMessage
    {
        public String Aktienname { get; set; }
        public int AktienID { get; set; }
    }

    public class NeueDividendeGespeichertMessage
    {
        public bool Erfolgreich { get; set; }
        public string Message { get; set; }
    }
}
