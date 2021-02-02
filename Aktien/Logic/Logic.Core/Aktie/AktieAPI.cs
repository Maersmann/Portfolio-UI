using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.AktienModels;
using Aktien.Data.Model.DepotModels;
using Aktien.Logic.Core.DividendeLogic;
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

            new AktieRepository().Speichern(inAktie);
        }

        public void Aktualisieren(Aktie inAktie)
        {
            new AktieRepository().Update(inAktie);
        }

        public bool IstAkieVorhanden(String inISIN)
        {
            return new AktieRepository().IstVorhanden(inISIN);
        }

        public ObservableCollection<Aktie> LadeAlle()
        {
            return new AktieRepository().LadeAlle();
        }

        public Aktie LadeAnhandID(int inID)
        {
            return new AktieRepository().LadeAnhandID(inID);
        }

        public void Entfernen(Aktie inAktie)
        {
            new AktieRepository().Entfernen(inAktie);
        }

        public ObservableCollection<Dividende> LadeAlleDividendenDerAktie(int inAktieID)
        {
            return new DividendeAPI().LadeAlleFuerAktie(inAktieID);
        }

        public Dividende LadeDividendeDerAktie(int inID)
        {
            return new DividendeAPI().LadeAnhandID(inID);
        }

        public ObservableCollection<OrderHistory> LadeAlleOrdersDerAktie(int inAktieID)
        {
            return new OrderHistoryRepository().LadeAlleByAktieID(inAktieID);
        }

        public OrderHistory LadeOrderDerAktie(int OrderID)
        {
            return new OrderHistoryRepository().LadeByID(OrderID);
        }

        public ObservableCollection<DividendeErhalten> LadeAlleErhalteneDividenden(int inAktieID)
        {
            new DividendeRepository().LadeAlleFuerAktie(inAktieID);
            return new DividendeErhaltenRepository().LadeAllByAktieID(inAktieID);
        }
    }

    public class AktieSchonVorhandenException : Exception
    {

    }
}
