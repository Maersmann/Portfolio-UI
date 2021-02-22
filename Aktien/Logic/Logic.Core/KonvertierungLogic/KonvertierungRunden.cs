using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Model.WertpapierEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.KonvertierungLogic
{
    public class KonvertierungRunden
    {
        public void Start()
        {
            var dwRepo = new DepotWertpapierRepository();
            IList<DepotWertpapier> depotList = dwRepo.LoadAll();

            depotList.ToList().ForEach(e =>
            {
                e.BuyIn = Math.Round(e.BuyIn, 3, MidpointRounding.AwayFromZero);
                if (e.BuyIn >= 2)
                    e.BuyIn = Math.Round(e.BuyIn, 2, MidpointRounding.AwayFromZero);

                dwRepo.Speichern(e.ID, e.Anzahl, e.BuyIn, e.WertpapierID, e.DepotID);
            });

            var dRepo = new DividendeRepository();
            IList<Dividende> dividendeList = dRepo.LadeAlle();

            dividendeList.ToList().ForEach(e =>
            {
                if (e.BetragUmgerechnet.HasValue)
                {
                    e.BetragUmgerechnet = Math.Round(e.BetragUmgerechnet.Value, 2, MidpointRounding.AwayFromZero);
                    dRepo.Speichern(e.ID, e.Betrag, e.Datum, e.WertpapierID, e.Waehrung, e.BetragUmgerechnet, Data.Types.DividendenTypes.DividendenRundungTypes.Normal);
                }
                
            });


        }
    }
}
