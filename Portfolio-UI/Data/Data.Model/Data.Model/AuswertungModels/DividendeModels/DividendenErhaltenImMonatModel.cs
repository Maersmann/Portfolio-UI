using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels.DividendeModels
{
    public class DividendenErhaltenImMonatModel
    {
        public int Monat { get; set; }
        public int Jahr { get; set; }
        public double Netto { get; set; }
        public double Brutto { get; set; }
        public double BruttoSonderDividende { get; set; }
        public double NettoSonderDividende { get; set; }
        public double NettoGesamt { get; set; }
        public double BruttoGesamt { get; set; }
        public IList<DividendenErhaltenImMonatDividendeModel> Dividenden { get; set; }
    }

    public class DividendenErhaltenImMonatDividendeModel
    {
        public double Netto { get; set; }
        public double Brutto { get; set; }
        public DateTime Datum { get; set; }
        public string Wertpapier { get; set; }
        public bool Sonderdividende { get; set; }

    }
}
