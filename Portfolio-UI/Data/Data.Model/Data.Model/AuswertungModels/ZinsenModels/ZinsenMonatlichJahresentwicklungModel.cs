using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels.ZinsenModels
{
    public class ZinsenMonatlichJahresentwicklungModel
    {
        public int Jahr { get; set; }
        public IList<ZinsenMonatlichJahresentwicklungWerteModel> Werte { get; set; }

        public ZinsenMonatlichJahresentwicklungModel()
        {
            Werte = new List<ZinsenMonatlichJahresentwicklungWerteModel>();
        }
    }

    public class ZinsenMonatlichJahresentwicklungWerteModel
    {
        public int Monat { get; set; }
        public Decimal Gesamt { get; set; }
        public Decimal Erhalten { get; set; }
    }
}