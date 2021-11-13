using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels
{
    public class SteuerMonatJahresVergleichAuswertungModel
    {
        public int Jahr { get; set; }
        public IList<SteuerMonatJahresVergleichMonatsWertModel> Monatswerte { get; set; }

        public SteuerMonatJahresVergleichAuswertungModel()
        {
            Monatswerte = new List<SteuerMonatJahresVergleichMonatsWertModel>();
        }
    }

    public class SteuerMonatJahresVergleichMonatsWertModel
    {
        public int Monat { get; set; }
        public Double Betrag { get; set; }
    }
}
