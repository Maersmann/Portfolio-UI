using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.Interfaces;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.WertpapierLogic
{
    public class EtfAPI : IAPI<Wertpapier>
    {
        public void Speichern(Wertpapier wertpapier)
        {
            if (IstAkieVorhanden(wertpapier.ISIN))
                throw new WertpapierSchonVorhandenException();

            wertpapier.WertpapierTyp = WertpapierTypes.ETF;

            new WertpapierRepository().Speichern(wertpapier);
            new ETFInfoRepository().Speichern(null, wertpapier.ETFInfo.Emittent, wertpapier.ETFInfo.ProfitTyp, wertpapier.ID);
        }

        public void Aktualisieren(Wertpapier wertpapier)
        {
            new WertpapierRepository().Speichern(wertpapier.ID, wertpapier.Name, wertpapier.ISIN, wertpapier.WKN, WertpapierTypes.ETF);
            new ETFInfoRepository().Speichern(wertpapier.ETFInfo.ID, wertpapier.ETFInfo.Emittent, wertpapier.ETFInfo.ProfitTyp, wertpapier.ID);
        }

        public bool IstAkieVorhanden(String isin)
        {
            return new WertpapierRepository().IstVorhanden(isin);
        }

        public ObservableCollection<Wertpapier> LadeAlle()
        {
            return new WertpapierRepository().LadeAlleByWertpapierTyp(WertpapierTypes.ETF);
        }

        public Wertpapier Lade(int id)
        {
            return new WertpapierRepository().LadeAnhandID(id);
        }

        public void Entfernen(Wertpapier wertpapier)
        {
            if (new DepotWertpapierRepository().IstWertpapierInDepotVorhanden(wertpapier.ID))
                throw new WertpapierInDepotVorhandenException();

            new WertpapierRepository().Entfernen(wertpapier);
        }

        public void Entfernen(int id)
        {
            throw new NotImplementedException();
        }
    }
}
