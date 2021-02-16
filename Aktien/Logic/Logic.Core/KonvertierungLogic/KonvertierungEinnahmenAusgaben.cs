using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Data.Types.DepotTypes;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Core.DividendeLogic.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.KonvertierungLogic
{
    public class KonvertierungEinnahmenAusgaben
    {
        public void Start()
        {
            IList<OrderHistory> OrderList = new OrderHistoryRepository().LadeAlle();

            var depotAPI = new DepotAPI();

            OrderList.ToList().ForEach(e =>
            {
                if (e.BuySell.Equals(BuySell.Buy))
                {
                    var Betrag = (e.Preis * e.Anzahl) + e.Fremdkostenzuschlag.GetValueOrDefault(0);
                    depotAPI.NeueAusgabe(Betrag, e.Orderdatum, AusgabenArtTypes.Kauf, 1, e.ID, "");
                }
                else
                {
                    var Betrag = (e.Preis * e.Anzahl) - e.Fremdkostenzuschlag.GetValueOrDefault(0);
                    depotAPI.NeueEinnahme(Betrag, e.Orderdatum, EinnahmeArtTypes.Verkauf, 1, e.ID, "");
                }

            });

            new DividendeRepository().LadeAlle();
            IList <DividendeErhalten> dividendeList = new DividendeErhaltenRepository().LadeAlle();

            dividendeList.ToList().ForEach(e =>
            {
                var EuroBetrag = e.GesamtNetto;
                if (!e.Dividende.Waehrung.Equals(Waehrungen.Euro))
                {
                    EuroBetrag = new DividendenBerechnungen().BetragUmgerechnet(EuroBetrag, e.Umrechnungskurs);
                }

                depotAPI.NeueEinnahme(EuroBetrag, e.Datum, EinnahmeArtTypes.Dividende, 1, e.ID, "");
            });


        }
    }
}
