using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DepotModels
{
    public class ErhalteneDividendeEintragenWerteModel
    {
        public string Bemessungsgrundlage { get; set; } 
        public string SteuerVorZwischensumme { get; set; }
        public string SteuerNachZwischensumme { get; set; }
        public string Zwischensumme { get; set; }
        public string Erhalten { get; set; }
        public string Bestand { get; set; }
        public string BetragGerundet { get; set; }
    }
}
