using Aktien.Data.Types.DividendenTypes;
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
        public double GesamtNetto { get; set; }
        public double GesamtBrutto { get; set; }
        public double? GesamtNettoUmgerechnetErhalten { get; set; }
        public double? GesamtNettoUmgerechnetErmittelt { get; set; }
        public double? Quellensteuer { get; set; }
        public double Bestand { get; set; }
        public double? Umrechnungskurs { get; set; }
        public DividendenRundungTypes RundungArt { get; set; }
        public double Dividende_Betrag { get; set; }
        public DateTime Dividende_Zahldatum { get; set; }
    }
}
