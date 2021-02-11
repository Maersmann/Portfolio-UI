using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.WertpapierEntitys;
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
        public void Speichern(int inWertpapierID, int inDividendeID, DateTime inDatum, Double? inQuellensteuer, Double? inUmrechnungskurs, int inBestand, double inGesamtBrutto, double inGesamtNetto)
        {
            new DividendeErhaltenRepository().Speichern(null, inDatum, inQuellensteuer, inUmrechnungskurs, inGesamtBrutto, inGesamtNetto, inBestand, inDividendeID, inWertpapierID);
        }

        public ObservableCollection<DividendeErhalten> LadeAlleFuerAktie(int inWertpapierID)
        {
            new DividendeRepository().LadeAlleFuerAktie(inWertpapierID);
            return new DividendeErhaltenRepository().LadeAllByWertpapierID(inWertpapierID);
        }

        public DividendeErhalten LadeAnhandID(int inID)
        {
            return new DividendeErhaltenRepository().LadeByID(inID);
        }

        public void Entfernen(int inID)
        {
            new DepotAPI().EntferneNeueEinnahme(null, inID);
            new DividendeErhaltenRepository().Entfernen(inID);
        }
    }
}
