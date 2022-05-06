using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels.DividendeModels
{
    public class DividendeMonatlichJahresentwicklungModel
    {
        public int Jahr { get; set; }
        public IList<DividendeMonatlichJahresentwicklungWerteModel> Werte { get; set; }

        public DividendeMonatlichJahresentwicklungModel()
        {
            Werte = new List<DividendeMonatlichJahresentwicklungWerteModel>();
        }
    }

    public class DividendeMonatlichJahresentwicklungWerteModel
    {
        public int Monat { get; set; }
        public double Brutto { get; set; }
        public double Netto { get; set; }
    }

}
