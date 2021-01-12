using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.AktieModels;
using Aktien.Data.Model.DepotModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.AktieLogic
{
    public class AktieAPI
    {
        public void Speichern(Aktie inAktie)
        {
            if (IstAkieVorhanden( inAktie.ISIN ))
            {
                throw new AktieSchonVorhandenException();
            }

            var Repo = new AktieRepository();
            Repo.Speichern(inAktie);
        }

        public void Update(Aktie inAktie)
        {
            var Repo = new AktieRepository();
            Repo.Update(inAktie);
        }

        public bool IstAkieVorhanden(String inISIN)
        {
            var Repo = new AktieRepository();
            return Repo.IstVorhanden(inISIN);
        }

        public ObservableCollection<Aktie> LadeAlle()
        {
            var Repo = new AktieRepository();
            return Repo.LadeAlle();
        }

        public Aktie LadeAnhandID(int inID)
        {
            var Repo = new AktieRepository();
            return Repo.LadeAnhandID(inID);
        }

        public void Entfernen(Aktie inAktie)
        {
            var Repo = new AktieRepository();
            Repo.Entfernen(inAktie);
        }

        public ObservableCollection<OrderHistory> LadeAlleOrdersDerAktie(int inAktieID)
        {
            return new OrderHistoryRepository().LadeAlleByAktieID(inAktieID);
        }
    }

    public class AktieSchonVorhandenException : Exception
    {

    }
}
