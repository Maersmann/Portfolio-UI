using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.WertpapierModels
{
    public class SplitEintragenDTO
    {
        public int WertpapierID { get; set; }
        public int Verhaeltnis { get; set; }
        public DateTime? Datum { get; set; }
    }
}
