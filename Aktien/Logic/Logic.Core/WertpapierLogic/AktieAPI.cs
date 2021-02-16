using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Model.DepotEntitys;
using Aktien.Logic.Core.DividendeLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aktien.Data.Types;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;

namespace Aktien.Logic.Core.WertpapierLogic
{
    public class AktieAPI
    {
        public void Speichern(Wertpapier inAktie)
        {
            if (IstAkieVorhanden( inAktie.ISIN ))
                throw new WertpapierSchonVorhandenException();

            new WertpapierRepository().Speichern(null, inAktie.Name, inAktie.ISIN, inAktie.WKN, WertpapierTypes.Aktie);
        }

        public void Aktualisieren(Wertpapier inAktie)
        {
            new WertpapierRepository().Speichern(inAktie.ID, inAktie.Name, inAktie.ISIN, inAktie.WKN, WertpapierTypes.Aktie);
        }

        public bool IstAkieVorhanden(String inISIN)
        {
            return new WertpapierRepository().IstVorhanden(inISIN);
        }

        public ObservableCollection<Wertpapier> LadeAlle()
        {
            return new WertpapierRepository().LadeAlleByWertpapierTyp(WertpapierTypes.Aktie);
        }

        public Wertpapier LadeAnhandID(int inID)
        {
            return new WertpapierRepository().LadeAnhandID(inID);
        }

        public void Entfernen(Wertpapier inAktie)
        {
            if (new DepotWertpapierRepository().IstWertpapierInDepotVorhanden( inAktie.ID) )
                throw new WertpapierInDepotVorhandenException();

            new WertpapierRepository().Entfernen(inAktie);
        }

        public ObservableCollection<Dividende> LadeAlleDividendenDerAktie(int inWertpapierID)
        {
            return new DividendeAPI().LadeAlleFuerWertpapier(inWertpapierID);
        }

        public Dividende LadeDividendeDerAktie(int inID)
        {
            return new DividendeAPI().LadeAnhandID(inID);
        }

        public ObservableCollection<OrderHistory> LadeAlleOrdersDerAktie(int inWertpapierID)
        {
            return new OrderHistoryRepository().LadeAlleByWertpapierID(inWertpapierID);
        }

        public OrderHistory LadeOrderDerAktie(int OrderID)
        {
            return new OrderHistoryRepository().LadeByID(OrderID);
        }

        public ObservableCollection<DividendeErhalten> LadeAlleErhalteneDividenden(int inWertpapierID)
        {
            new DividendeRepository().LadeAlleFuerAktie(inWertpapierID);
            return new DividendeErhaltenRepository().LadeAllByWertpapierID(inWertpapierID);
        }
    }
}
