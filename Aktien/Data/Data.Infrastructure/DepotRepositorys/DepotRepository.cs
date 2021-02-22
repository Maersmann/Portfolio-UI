using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.DepotEntitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aktien.Data.Infrastructure.DepotRepositorys
{
    public class DepotRepository : BaseRepository
    {
        public void Speichern(int? iD, string bezeichnung, Double? gesamtAusgaben, Double? gesamtEinnahmen)
        {
            var Entity = new Depot();

            if (iD.GetValueOrDefault(0).Equals(0))
                iD = null;

            if (iD.HasValue)
                Entity = repo.Depots.Find(iD.Value);

            Entity.Bezeichnung = bezeichnung;
            Entity.GesamtAusgaben = Math.Round(gesamtAusgaben.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);
            Entity.GesamtEinahmen = Math.Round(gesamtEinnahmen.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);

            if (!iD.HasValue)
                repo.Depots.Add(Entity);

            repo.SaveChanges();
        }

        public void Speichern(Depot depot)
        {
            if (depot.ID == 0)
                repo.Depots.Add(depot);

            depot.GesamtAusgaben = Math.Round(depot.GesamtAusgaben.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);
            depot.GesamtEinahmen = Math.Round(depot.GesamtEinahmen.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);

            repo.SaveChanges();
        }

        public Depot LoadByID( int iD )
        {
            return repo.Depots.Find(iD);
        }
    }
}
