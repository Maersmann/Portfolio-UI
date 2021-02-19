using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.WertpapierLogic
{
    public class EtfAPI
    {
        public void Speichern(Wertpapier inWertpapier)
        {
            if (IstAkieVorhanden(inWertpapier.ISIN))
                throw new WertpapierSchonVorhandenException();

            inWertpapier.WertpapierTyp = WertpapierTypes.ETF;

            new WertpapierRepository().Speichern(inWertpapier);
            new ETFInfoRepository().Speichern(null, inWertpapier.ETFInfo.Emittent, inWertpapier.ETFInfo.ProfitTyp, inWertpapier.ID);
        }

        public void Aktualisieren(Wertpapier inWertpapier)
        {
            new WertpapierRepository().Speichern(inWertpapier.ID, inWertpapier.Name, inWertpapier.ISIN, inWertpapier.WKN, WertpapierTypes.ETF);
            new ETFInfoRepository().Speichern(inWertpapier.ETFInfo.ID, inWertpapier.ETFInfo.Emittent, inWertpapier.ETFInfo.ProfitTyp, inWertpapier.ID);
        }

        public bool IstAkieVorhanden(String inISIN)
        {
            return new WertpapierRepository().IstVorhanden(inISIN);
        }

        public ObservableCollection<Wertpapier> LadeAlle()
        {
            return new WertpapierRepository().LadeAlleByWertpapierTyp(WertpapierTypes.ETF);
        }

        public Wertpapier LadeAnhandID(int inID)
        {
            return new WertpapierRepository().LadeAnhandID(inID);
        }

        public void Entfernen(Wertpapier inWertpapier)
        {
            if (new DepotWertpapierRepository().IstWertpapierInDepotVorhanden(inWertpapier.ID))
                throw new WertpapierInDepotVorhandenException();

            new WertpapierRepository().Entfernen(inWertpapier);
        }
    }
}
