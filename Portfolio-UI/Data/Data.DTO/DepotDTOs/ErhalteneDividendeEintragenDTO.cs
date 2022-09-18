using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using Data.Model.SteuerModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Data.DTO.DepotDTOs
{
    public class ErhalteneDividendeEintragenDTO
    {
        public int WertpapierId { get; set; }
        public double? Umrechnungskurs { get; set; }
        public DateTime ExDatum { get; set; }
        public DateTime ZahlDatum { get; set; }
        public double Betrag { get; set; }
        public DividendenRundungTypes RundungDividende { get; set; }
        public DividendenRundungTypes RundungErhalten { get; set; }
        public Waehrungen Waehrung { get; set; }
        public SteuergruppeModel Steuer { get; set; }
    }
}
