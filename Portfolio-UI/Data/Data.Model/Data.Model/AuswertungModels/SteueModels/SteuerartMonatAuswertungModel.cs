using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels
{
    public class SteuerartMonatAuswertungModel
    {
        public DateTime Datum { get; set; }
        public IList<SteuerartMonatAuswertungSteuerartModel> Steuerarten { get; set; }

        public SteuerartMonatAuswertungModel()
        {
            Steuerarten = new List<SteuerartMonatAuswertungSteuerartModel>();
        }

    }

    public class SteuerartMonatAuswertungSteuerartModel
    {
        public string Steuerart { get; set; }
        public double Betrag { get; set; }
    }
}
