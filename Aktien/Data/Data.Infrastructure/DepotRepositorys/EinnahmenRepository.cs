using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types.DepotTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aktien.Data.Infrastructure.DepotRepositorys
{
    public class EinnahmenRepository : BaseRepository
    {
        public void Speichern(int? inID, Double inBetrag, DateTime inDatum, EinnahmeArtTypes inTyp, int inDepotID, int? inHerkunftID)
        {
            var Entity = new Einnahme();

            if (inID.GetValueOrDefault(0).Equals(0))
                inID = null;

            if (inID.HasValue)
                Entity = repo.Einnahmen.Find(inID.Value);

            Entity.Art = inTyp;
            Entity.Betrag = inBetrag;
            Entity.Datum = inDatum;
            Entity.DepotID = inDepotID;
            Entity.HerkunftID = inHerkunftID;

            if (!inID.HasValue)
                repo.Einnahmen.Add(Entity);

            repo.SaveChanges();
        }

        public void Speichern(Einnahme inEinnahme)
        {
            if (inEinnahme.ID == 0)
                repo.Einnahmen.Add(inEinnahme);
            repo.SaveChanges();
        }

        public void Entfernen( Einnahme inEinahme )
        {
            repo.Einnahmen.Remove(inEinahme);
            repo.SaveChanges();
        }

        public Einnahme LoadByID(int inID)
        {
            return repo.Einnahmen.Find(inID);
        }

        public Einnahme LoadByHerkunftID(int inHerkunftID)
        {
            return repo.Einnahmen.Where(e => e.HerkunftID.HasValue).Where(e => e.HerkunftID.Value.Equals(inHerkunftID)).FirstOrDefault();
        }
    }
}
