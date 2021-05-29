using Data.Types.SteuerTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.SteuerModels
{
    public class SteuerartModel
    {
        public int ID { get; set; }
        public string Bezeichnung { get; set; }
        public SteuerberechnungZwischensumme BerechnungZwischensumme { get; set; }
    }
}
