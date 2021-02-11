using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.DepotEntitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aktien.Data.Infrastructure.DepotRepositorys
{
    public class DepotRepository : BaseRepository
    {
        public void Speichern(int? inID, string inBezeichnung, Double? inGesamtAusgaben, Double? inGesamtEinnahmen)
        {
            var Entity = new Depot();

            if (inID.GetValueOrDefault(0).Equals(0))
                inID = null;

            if (inID.HasValue)
                Entity = repo.Depots.Find(inID.Value);

            Entity.Bezeichnung = inBezeichnung;
            Entity.GesamtAusgaben = inGesamtAusgaben;
            Entity.GesamtEinahmen = inGesamtEinnahmen;

            if (!inID.HasValue)
                repo.Depots.Add(Entity);

            repo.SaveChanges();
        }

        public void Speichern(Depot inDepot)
        {
            if (inDepot.ID == 0)
                repo.Depots.Add(inDepot);
            repo.SaveChanges();
        }

        public Depot LoadByID( int inID )
        {
            return repo.Depots.Find(inID);
        }
    }
}
