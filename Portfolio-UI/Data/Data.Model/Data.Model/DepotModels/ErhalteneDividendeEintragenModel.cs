using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using Data.Model.DividendeModels;
using Data.Model.SteuerModels;
using Data.Types.DividendenTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DepotModels
{
    public class ErhalteneDividendeEintragenModel
    {
        public int WertpapierID { get; set; }
        public string Bestand { get; set; }
        public string Umrechnungskurs { get; set; }
        public DateTime? Exdatum { get; set; }
        public DateTime? Zahldatum { get; set; }
        public string Betrag { get; set; }
        public DividendenRundungTypes RundungArtDividende { get; set; }
        public DividendenRundungTypes RundungArtErhalten { get; set; }
        public SteuergruppeModel Steuer { get; set; }
        public Waehrungen Waehrung { get; set; }

        public ErhalteneDividendeEintragenModel()
        {
            Bestand = "";
            Umrechnungskurs = "";
            Bestand = "";
        }
    }
}
