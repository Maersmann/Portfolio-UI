using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.DepotEntitys;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.DepotLogic
{
    public class DepotWertpapierAPI
    {
        public DepotWertpapier LadeByWertpapierID(int wertpapierId)
        {
            return new DepotWertpapierRepository().LadeByWertpapierID(wertpapierId);
        }

        public void Aktualisieren(DepotWertpapier entity)
        {
            new DepotWertpapierRepository().Speichern(entity);
        }
    }
}
