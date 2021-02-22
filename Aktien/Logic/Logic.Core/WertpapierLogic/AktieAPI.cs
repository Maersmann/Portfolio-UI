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
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Logic.Core.WertpapierLogic.Exceptions;

namespace Aktien.Logic.Core.WertpapierLogic
{
    public class AktieAPI
    {
        public void Speichern(Wertpapier aktie)
        {
            if (IstAkieVorhanden( aktie.ISIN ))
                throw new WertpapierSchonVorhandenException();

            new WertpapierRepository().Speichern(null, aktie.Name, aktie.ISIN, aktie.WKN, WertpapierTypes.Aktie);
        }

        public void Aktualisieren(Wertpapier aktie)
        {
            new WertpapierRepository().Speichern(aktie.ID, aktie.Name, aktie.ISIN, aktie.WKN, WertpapierTypes.Aktie);
        }

        public bool IstAkieVorhanden(String isin)
        {
            return new WertpapierRepository().IstVorhanden(isin);
        }

        public ObservableCollection<Wertpapier> LadeAlle()
        {
            return new WertpapierRepository().LadeAlleByWertpapierTyp(WertpapierTypes.Aktie);
        }

        public Wertpapier LadeAnhandID(int id)
        {
            return new WertpapierRepository().LadeAnhandID(id);
        }

        public void Entfernen(Wertpapier aktie)
        {
            if (new DepotWertpapierRepository().IstWertpapierInDepotVorhanden(aktie.ID) )
                throw new WertpapierInDepotVorhandenException();

            new WertpapierRepository().Entfernen(aktie);
        }

        public ObservableCollection<Dividende> LadeAlleDividendenDerAktie(int wertpapierID)
        {
            return new DividendeAPI().LadeAlleFuerWertpapier(wertpapierID);
        }

        public Dividende LadeDividendeDerAktie(int id)
        {
            return new DividendeAPI().LadeAnhandID(id);
        }

        public ObservableCollection<OrderHistory> LadeAlleOrdersDerAktie(int wertpapierID)
        {
            return new OrderHistoryRepository().LadeAlleByWertpapierID(wertpapierID);
        }

        public OrderHistory LadeOrderDerAktie(int orderID)
        {
            return new OrderHistoryRepository().LadeByID(orderID);
        }

        public ObservableCollection<DividendeErhalten> LadeAlleErhalteneDividenden(int wertpapierID)
        {
            new DividendeRepository().LadeAlleFuerAktie(wertpapierID);
            return new DividendeErhaltenRepository().LadeAllByWertpapierID(wertpapierID);
        }
    }
}
