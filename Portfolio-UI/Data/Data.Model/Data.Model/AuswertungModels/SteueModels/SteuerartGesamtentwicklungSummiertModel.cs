using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels.SteueModels
{
    public class SteuerartGesamtentwicklungSummiertModel
    {
        public DateTime Datum { get; set; }
        public IList<SteuerartGesamtentwicklungSummiertSteuerartModel> Steuerarten { get; set; }

        public SteuerartGesamtentwicklungSummiertModel()
        {
            Steuerarten = new List<SteuerartGesamtentwicklungSummiertSteuerartModel>();
        }

    }

    public class SteuerartGesamtentwicklungSummiertSteuerartModel
    {
        public String Steuerart { get; set; }
        public double Betrag { get; set; }
    }
}
