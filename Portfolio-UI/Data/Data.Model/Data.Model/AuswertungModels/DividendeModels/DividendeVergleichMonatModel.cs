using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels
{
    public class DividendeVergleichMonatModel
    {
        public int Jahr { get; set; }
        public IList<DividendeMonatJahresVergleichMonatsWertAuswertungModel> Monatswerte { get; set; }

        public DividendeVergleichMonatModel()
        {
            Monatswerte = new List<DividendeMonatJahresVergleichMonatsWertAuswertungModel>();
        }
    }

    public class DividendeMonatJahresVergleichMonatsWertAuswertungModel
    {
        public int Monat { get; set; }
        public double Netto { get; set; }
        public double Brutto { get; set; }
    }
}
