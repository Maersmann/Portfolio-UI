using Aktien.Data.Infrastructure.Aktien.Repository;
using Aktien.Data.Model.AktieModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.DividendeLogic
{
    public class DividendeAPI
    {
        public void SpeicherNeueDividende(Double inBetrag, DateTime inDatum, int inAktieID)
        {
            var DividendeRepo = new DividendeRepository();
            DividendeRepo.Speichern(inBetrag, inDatum, inAktieID);
        }

        public void UpdateDividende(Double inBetrag, DateTime inDatum, int inID)
        {
            var DividendeRepo = new DividendeRepository();
            DividendeRepo.Update(inBetrag,inDatum,inID);
        }

        public void Entfernen(int inID)
        {
            var DividendeRepo = new DividendeRepository();
            DividendeRepo.Entfernen( inID);
        }

        public ObservableCollection<Dividende> LadeAlleFuerAktie(int inAktieID)
        {
            var DividendeRepo = new DividendeRepository();
            return DividendeRepo.LadeAlleFuerAktie(inAktieID);
        }

        public Dividende LadeAnhandID(int inID)
        {
            var DividendeRepo = new DividendeRepository();
            return DividendeRepo.LadeAnhandID(inID);
        }
    }
}
