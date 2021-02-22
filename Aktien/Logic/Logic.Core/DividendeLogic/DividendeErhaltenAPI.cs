using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Logic.Core.Depot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.DividendeLogic
{
    public class DividendeErhaltenAPI
    {
        public void Speichern(int wertpapierID, int dividendeID, DateTime datum, Double? quellensteuer, Double? umrechnungskurs, int bestand, double gesamtBrutto, double gesamtNetto, DividendenRundungTypes typ, Double? gesamtNettoUmgerechnetErhalten, Double? gesamtNettoUmgerechnetErmittelt)
        {
            new DividendeErhaltenRepository().Speichern(null, datum, quellensteuer, umrechnungskurs, gesamtBrutto, gesamtNetto, bestand, dividendeID, wertpapierID, typ, gesamtNettoUmgerechnetErhalten, gesamtNettoUmgerechnetErmittelt);
        }

        public ObservableCollection<DividendeErhalten> LadeAlleFuerAktie(int wertpapierID)
        {
            new DividendeRepository().LadeAlleFuerAktie(wertpapierID);
            return new DividendeErhaltenRepository().LadeAllByWertpapierID(wertpapierID);
        }

        public DividendeErhalten LadeAnhandID(int id)
        {
            return new DividendeErhaltenRepository().LadeByID(id);
        }

        public void Entfernen(int id)
        {
            new DepotAPI().EntferneNeueEinnahme(null, id, Data.Types.DepotTypes.EinnahmeArtTypes.Dividende );
            new DividendeErhaltenRepository().Entfernen(id);
        }
    }
}
