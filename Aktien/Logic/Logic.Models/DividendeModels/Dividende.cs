using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models.DividendeModels
{
    public class Dividende
    {
        public DateTime? Datum { get; set; }
        public Double? Betrag { get; set; }
        public String Aktienname { get; set; }
        public int? AktienID { get; set; }
    }
}
