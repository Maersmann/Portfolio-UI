using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels.ZinsenModels
{
    public class ZinsenVergleichMonatModel
    {
        public int Jahr { get; set; }
        public IList<ZinsenMonatJahresVergleichMonatsWertModel> Monatswerte { get; set; }

        public ZinsenVergleichMonatModel()
        {
            Monatswerte = new List<ZinsenMonatJahresVergleichMonatsWertModel>();
        }
    }

    public class ZinsenMonatJahresVergleichMonatsWertModel
    {
        public int Monat { get; set; }
        public Decimal Gesamt { get; set; }
        public Decimal Erhalten { get; set; }
    }
}
