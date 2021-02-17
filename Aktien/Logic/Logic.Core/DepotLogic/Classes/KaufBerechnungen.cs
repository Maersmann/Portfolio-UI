using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.DepotLogic.Classes
{
    public class KaufBerechnungen
    { 
        public Double BuyInAktieGekauft(Double inBuyIn, double inAlteAnzahl, double inNeueGesamtAnzahl, double inPreis, double inNeueAnzahl, double? inFremdkosten)
        {
            var BuyIn = Math.Round(((inBuyIn * inAlteAnzahl) + ((inPreis * inNeueAnzahl) + inFremdkosten.GetValueOrDefault(0))) / inNeueGesamtAnzahl, 3, MidpointRounding.AwayFromZero);

            if ( BuyIn >= 2 ) 
                
                BuyIn = Math.Round(((inBuyIn * inAlteAnzahl) + ((inPreis * inNeueAnzahl) + inFremdkosten.GetValueOrDefault(0))) / inNeueGesamtAnzahl, 2, MidpointRounding.AwayFromZero);
            return BuyIn;
        }

        public Double BuyInAktieEntfernt(Double inBuyIn, double inAlteAnzahl, double inNeueGesamtAnzahl, double inPreis, double inNeueAnzahl, double? inFremdkosten)
        {
            var BuyIn = Math.Round(((inBuyIn * inAlteAnzahl) - ((inPreis * inNeueAnzahl) - inFremdkosten.GetValueOrDefault(0))) / inNeueGesamtAnzahl, 3, MidpointRounding.AwayFromZero);

            if (BuyIn >= 2)

                BuyIn = Math.Round(((inBuyIn * inAlteAnzahl) - ((inPreis * inNeueAnzahl) - inFremdkosten.GetValueOrDefault(0))) / inNeueGesamtAnzahl, 2, MidpointRounding.AwayFromZero);
            return BuyIn;
        }
    }
}
