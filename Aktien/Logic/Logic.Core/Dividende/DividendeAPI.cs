using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.WertpapierModels;
using Aktien.Data.Types;
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
        public void Speichern(Double inBetrag, DateTime inDatum, int inWertpapierID, Waehrungen inWaehrung, Double? inBetragUmgerechnet)
        {
            var DividendeRepo = new DividendeRepository();
            DividendeRepo.Speichern(null, inBetrag, inDatum, inWertpapierID, inWaehrung, inBetragUmgerechnet);
        }

        public void Aktualisiere(Double inBetrag, DateTime inDatum, int inID, Waehrungen inWaehrung, Double? inBetragUmgerechnet)
        {
            var DividendeRepo = new DividendeRepository();
            DividendeRepo.Speichern(inID, inBetrag, inDatum, null , inWaehrung, inBetragUmgerechnet);
        }

        public void Entfernen(int inID)
        {
            var DividendeRepo = new DividendeRepository();
            DividendeRepo.Entfernen( inID);
        }

        public ObservableCollection<Dividende> LadeAlleFuerAktie(int inWertpapierID)
        {
            var DividendeRepo = new DividendeRepository();
            return DividendeRepo.LadeAlleFuerAktie(inWertpapierID);
        }

        public Dividende LadeAnhandID(int inID)
        {
            var DividendeRepo = new DividendeRepository();
            return DividendeRepo.LadeAnhandID(inID);
        }
    }
}
