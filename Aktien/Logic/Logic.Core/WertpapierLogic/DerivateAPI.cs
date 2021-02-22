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
    public class DerivateAPI
    {
        public void Speichern(Wertpapier wertpapier)
        {
            if (IstAkieVorhanden(wertpapier.ISIN))
                throw new WertpapierSchonVorhandenException();

            new WertpapierRepository().Speichern(null, wertpapier.Name, wertpapier.ISIN, wertpapier.WKN, WertpapierTypes.Derivate);
        }

        public void Aktualisieren(Wertpapier wertpapier)
        {
            new WertpapierRepository().Speichern(wertpapier.ID, wertpapier.Name, wertpapier.ISIN, wertpapier.WKN, WertpapierTypes.Derivate);
        }

        public bool IstAkieVorhanden(String isin)
        {
            return new WertpapierRepository().IstVorhanden(isin);
        }

        public ObservableCollection<Wertpapier> LadeAlle()
        {
            return new WertpapierRepository().LadeAlleByWertpapierTyp( WertpapierTypes.Derivate);
        }

        public Wertpapier LadeAnhandID(int id)
        {
            return new WertpapierRepository().LadeAnhandID(id);
        }

        public void Entfernen(Wertpapier wertpapier)
        {
            if (new DepotWertpapierRepository().IstWertpapierInDepotVorhanden(wertpapier.ID))
                throw new WertpapierInDepotVorhandenException();

            new WertpapierRepository().Entfernen(wertpapier);
        }
    }
}
