using Aktien.Data.Types.DepotTypes;
using Aktien.Logic.Core.DepotLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.DepotLogic
{
    public class EinnahmeAusgabeAuswertungAPI
    {

        public EinnahmenAusgabenGesamtModel BerechneGesamtwerte()
        {
            var data = new EinnahmenAusgabenGesamtModel();

            var EinnahmeGroupByArtList =  new EinnahmenAPI().LadeAlle().GroupBy(e => e.Art );
            foreach (var group in EinnahmeGroupByArtList)
            {
                foreach (var einnahme in group)
                {
                    if (group.Key.Equals(EinnahmeArtTypes.Einzahlung))
                        data.EinnahmeEinzahlung += einnahme.Betrag;
                    if (group.Key.Equals(EinnahmeArtTypes.Dividende))
                        data.EinnahmeDividende += einnahme.Betrag;
                    if (group.Key.Equals(EinnahmeArtTypes.Verkauf))
                        data.EinnahmeVerkauf += einnahme.Betrag;
                }
            }

            var AusgabeGroupByArtList = new AusgabeAPI().LadeAlle().GroupBy(e => e.Art);
            foreach (var group in AusgabeGroupByArtList)
            {
                foreach (var ausgabe in group)
                {
                    if (group.Key.Equals(AusgabenArtTypes.Auszahlung))
                        data.AusgabeAuszahlung += ausgabe.Betrag;
                    if (group.Key.Equals(AusgabenArtTypes.Kauf))
                        data.AusgabeKauf += ausgabe.Betrag;
                }
            }

            return data;
        }
    }
}
