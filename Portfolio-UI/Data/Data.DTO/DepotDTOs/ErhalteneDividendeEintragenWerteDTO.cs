using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO.DepotDTOs
{
    public class ErhalteneDividendeEintragenWerteDTO
    {
        public double Bemessungsgrundlage { get; set; }
        public double SteuerVorZwischensumme { get; set; }
        public double SteuerNachZwischensumme { get; set; }
        public double Zwischensumme { get; set; }
        public double Erhalten { get; set; }
        public double Bestand { get; set; }
    }
}
