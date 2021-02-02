using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.AuswahlMessages
{
    public class DividendeAusgewaehltMessage
    { 
        public int ID { get; set; }
        public Double Betrag { get; set; }
        public DateTime Datum { get; set; }
    }
}
