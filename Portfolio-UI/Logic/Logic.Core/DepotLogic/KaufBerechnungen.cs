using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.DepotLogic
{
    public class KaufBerechnungen
    { 
        public double BuyInAktieGekauft(double buyIn, double alteAnzahl, double neueGesamtAnzahl, double preis, double neueAnzahl, double? fremdkosten, OrderTypes orderTypes)
        {
            var BuyIn = Math.Round(((buyIn * alteAnzahl) + ((preis * neueAnzahl) + fremdkosten.GetValueOrDefault(0))) / neueGesamtAnzahl, 3, MidpointRounding.AwayFromZero);

            if ( BuyIn >= 3 && orderTypes != OrderTypes.Sparplan )            
                BuyIn = Math.Round(((buyIn * alteAnzahl) + ((preis * neueAnzahl) + fremdkosten.GetValueOrDefault(0))) / neueGesamtAnzahl, 2, MidpointRounding.AwayFromZero);

            return BuyIn;
        }

        public double BuyInAktieEntfernt(double buyIn, double alteAnzahl, double neueGesamtAnzahl, double preis, double neueAnzahl, double? fremdkosten, OrderTypes orderTypes)
        {
            var BuyIn = Math.Round(((buyIn * alteAnzahl) - ((preis * neueAnzahl) - fremdkosten.GetValueOrDefault(0))) / neueGesamtAnzahl, 3, MidpointRounding.AwayFromZero);

            if (BuyIn >= 2 && orderTypes != OrderTypes.Sparplan)
                BuyIn = Math.Round(((buyIn * alteAnzahl) - ((preis * neueAnzahl) - fremdkosten.GetValueOrDefault(0))) / neueGesamtAnzahl, 2, MidpointRounding.AwayFromZero);

            return BuyIn;
        }
    }
}
