using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.Depot;
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
        public void Speichern(Double betrag, DateTime zahldatum, DateTime? exdatum, int wertpapierID, Waehrungen waehrung, Double? betragUmgerechnet, DividendenRundungTypes rundungTypes)
        {
            var DividendeRepo = new DividendeRepository();
            DividendeRepo.Speichern(null, betrag, zahldatum, exdatum, wertpapierID, waehrung, betragUmgerechnet, rundungTypes);
        }

        public void Aktualisiere(Double betrag, DateTime zahldatum, DateTime? exdatum, int iD, Waehrungen waehrung, Double? betragUmgerechnet, DividendenRundungTypes rundungTypes)
        {
            var DividendeRepo = new DividendeRepository();
            DividendeRepo.Speichern(iD, betrag, zahldatum,exdatum, null , waehrung, betragUmgerechnet, rundungTypes);

            if (new DividendeErhaltenRepository().IstDividendeErhalten(iD))
            {
                new DepotAPI().AktualisiereDividendeErhalten(new DividendeErhaltenRepository().LadeByDividendeID(iD));
            }
        }

        public void Speichern(Dividende dividende)
        {
            new DividendeRepository().Speichern(dividende);
        }

        public void Entfernen(int iD)
        {
            var DividendeRepo = new DividendeRepository();
            DividendeRepo.Entfernen(iD);
        }

        public ObservableCollection<Dividende> LadeAlleFuerWertpapier(int wertpapierID)
        {
            var DividendeRepo = new DividendeRepository();
            return DividendeRepo.LadeAlleFuerAktie(wertpapierID);
        }

        public ObservableCollection<Dividende> LadeAlleNichtErhaltendeFuerWertpapier(int wertpapierID)
        {
            return new DividendeRepository().LadeAlleNichtErhaltendeFuerWertpapier(wertpapierID) ;
        }

        public Dividende LadeAnhandID(int iD)
        {
            var DividendeRepo = new DividendeRepository();
            return DividendeRepo.LadeAnhandID(iD);
        }
    }
}
