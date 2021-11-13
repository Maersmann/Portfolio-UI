using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels
{
    public class DividendeWertpapierAuswertungModel
    {
        public string Bezeichnung { get; set; }
        public double Betrag { get; set; }
        public decimal ProzentBetragZuInvestition { get; set; }
    }
}
