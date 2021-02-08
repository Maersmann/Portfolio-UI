using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.WertpapierModels;
using Aktien.Data.Types;
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
        public void Speichern(Wertpapier inAktie)
        {
            if (IstAkieVorhanden(inAktie.ISIN))
                throw new WertpapierSchonVorhandenException();

            new WertpapierRepository().Speichern(null, inAktie.Name, inAktie.ISIN, inAktie.WKN, WertpapierTypes.ETF);
        }

        public void Aktualisieren(Wertpapier inAktie)
        {
            new WertpapierRepository().Speichern(inAktie.ID, inAktie.Name, inAktie.ISIN, inAktie.WKN, WertpapierTypes.ETF);
        }

        public bool IstAkieVorhanden(String inISIN)
        {
            return new WertpapierRepository().IstVorhanden(inISIN);
        }

        public ObservableCollection<Wertpapier> LadeAlle()
        {
            return new WertpapierRepository().LadeAlleETFs();
        }

        public Wertpapier LadeAnhandID(int inID)
        {
            return new WertpapierRepository().LadeAnhandID(inID);
        }

        public void Entfernen(Wertpapier inAktie)
        {
            new WertpapierRepository().Entfernen(inAktie);
        }
    }
}
