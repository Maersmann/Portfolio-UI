using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels
{
    public class DividendeMonatJahresVergleichAuswertungModel
    {
        public int Jahr { get; set; }
        public IList<DividendeMonatJahresVergleichMonatsWertAuswertungModel> Monatswerte { get; set; }

        public DividendeMonatJahresVergleichAuswertungModel()
        {
            Monatswerte = new List<DividendeMonatJahresVergleichMonatsWertAuswertungModel>();
        }
    }

    public class DividendeMonatJahresVergleichMonatsWertAuswertungModel
    {
        public int Monat { get; set; }
        public Double Betrag { get; set; }
    }
}
