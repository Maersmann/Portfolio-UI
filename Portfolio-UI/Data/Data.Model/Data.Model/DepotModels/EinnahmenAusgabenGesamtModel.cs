using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.DepotModels
{
    public  class EinnahmenAusgabenGesamtModel
    {
        private double einnahmeEinzahlung;
        private double einnahmeVerkauf;
        private double einnahmeDividende;
        private double ausgabeAuszahlung;
        private double ausgabeKauf;

        public Double EinnahmeEinzahlung { get { return einnahmeEinzahlung; } set { einnahmeEinzahlung = Math.Round(value,2,MidpointRounding.AwayFromZero);} }
        public Double EinnahmeVerkauf { get { return einnahmeVerkauf; } set { einnahmeVerkauf = Math.Round(value, 2, MidpointRounding.AwayFromZero); } }
        public Double EinnahmeDividende { get { return einnahmeDividende; } set { einnahmeDividende = Math.Round(value, 2, MidpointRounding.AwayFromZero); } }
        public Double EinnahmeGesamt { get { return EinnahmeDividende + EinnahmeEinzahlung + EinnahmeVerkauf; } }
        public Double AusgabeAuszahlung { get { return ausgabeAuszahlung; } set { ausgabeAuszahlung = Math.Round(value, 2, MidpointRounding.AwayFromZero); } }
        public Double AusgabeKauf { get { return ausgabeKauf; } set { ausgabeKauf = Math.Round(value, 2, MidpointRounding.AwayFromZero); } }
        public Double AusgabeGesamt { get { return AusgabeAuszahlung + AusgabeKauf; } }
        public Double Differenz { get { return Math.Round(EinnahmeGesamt - AusgabeGesamt, 2, MidpointRounding.AwayFromZero) ; } }
    }
}
