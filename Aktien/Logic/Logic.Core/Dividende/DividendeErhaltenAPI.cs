using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.AktienModels;
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
        public void Speichern(int inAktieID, int inDividendeID, DateTime inDatum, Double? inQuellensteuer, Double? inUmrechnungskurs, int inBestand, double inGesamtBrutto, double inGesamtNetto)
        {
            new DividendeErhaltenRepository().Speichern(null, inDatum, inQuellensteuer, inUmrechnungskurs, inGesamtBrutto, inGesamtNetto, inBestand, inDividendeID, inAktieID);
        }

        public void Aktualisiere(int inAktieID, int inDividendeID, DateTime inDatum, Double? inQuellensteuer, Double? inUmrechnungskurs, int inBestand, double inGesamtBrutto, double inGesamtNetto)
        {
            //var DividendeRepo = new DividendeRepository();
            //DividendeRepo.Update(inBetrag, inDatum, inID, inWaehrung, inBetragUmgerechnet);
        }

        public ObservableCollection<DividendeErhalten> LadeAlleFuerAktie(int inAktieID)
        {
            new DividendeRepository().LadeAlleFuerAktie(inAktieID);
            return new DividendeErhaltenRepository().LadeAllByAktieID(inAktieID);
        }

        public DividendeErhalten LadeAnhandID(int inID)
        {
            return new DividendeErhaltenRepository().LadeByID(inID);
        }

        public void Entfernen(int inID)
        {
            new DividendeErhaltenRepository().Entfernen(inID);
        }
    }
}
