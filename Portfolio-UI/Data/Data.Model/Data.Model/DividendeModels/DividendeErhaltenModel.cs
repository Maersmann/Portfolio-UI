using Aktien.Data.Types.DividendenTypes;
using Data.Model.SteuerModels;
using Data.Types.DividendenTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DividendeModels
{
    public class DividendeErhaltenModel
    {
        public int ID { get; set; }
        public int DividendeID { get; set; }
        public int WertpapierID { get; set; }
        public int? SteuergruppeID { get; set; }
        public double Bestand { get; set; }
        public double? Umrechnungskurs { get; set; }
        public DividendenRundungTypes RundungArt { get; set; }
        public SteuergruppeModel Steuer { get; set; }
        public DividendeModel Dividende { get; set; }
        public double Erhalten { get; set; }
        public double Bemessungsgrundlage { get; set; }
        public DividendeErhaltenArt Art { get; set; }
        public bool Aktualisiert { get; set; }
    }
}
