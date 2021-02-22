using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types.DepotTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Aktien.Data.Infrastructure.DepotRepositorys
{
    public class EinnahmenRepository : BaseRepository
    {
        public void Speichern(int? id, Double betrag, DateTime datum, EinnahmeArtTypes typ, int depotID, int? herkunftID, string beschreibung)
        {
            var Entity = new Einnahme();

            if (id.GetValueOrDefault(0).Equals(0))
                id = null;

            if (id.HasValue)
                Entity = repo.Einnahmen.Find(id.Value);

            Entity.Art = typ;
            Entity.Betrag = Math.Round(betrag, 2, MidpointRounding.AwayFromZero);
            Entity.Datum = datum;
            Entity.DepotID = depotID;
            Entity.HerkunftID = herkunftID;
            Entity.Beschreibung = beschreibung;

            if (!id.HasValue)
                repo.Einnahmen.Add(Entity);

            repo.SaveChanges();
        }

        public ObservableCollection<Einnahme> LoadAll()
        {
            return new ObservableCollection<Einnahme>(repo.Einnahmen.OrderByDescending(o => o.Datum).ToList());
        }

        public void Speichern(Einnahme einnahme)
        {
            if (einnahme.ID == 0)
                repo.Einnahmen.Add(einnahme);

            einnahme.Betrag = Math.Round(einnahme.Betrag, 2, MidpointRounding.AwayFromZero);

            repo.SaveChanges();
        }

        public void Entfernen( Einnahme einnahme)
        {
            repo.Einnahmen.Remove(einnahme);
            repo.SaveChanges();
        }

        public void EntferneAlle()
        {
            repo.Einnahmen.RemoveRange(repo.Einnahmen);
            repo.SaveChanges();
        }

        public Einnahme LoadByID(int id)
        {
            return repo.Einnahmen.Find(id);
        }

        public Einnahme LoadByHerkunftIDAndArt(int herkunftID, EinnahmeArtTypes typ)
        {
            return repo.Einnahmen.Where(e => e.HerkunftID.HasValue).Where(e => e.HerkunftID.Value.Equals(herkunftID)).Where(e => e.Art.Equals(typ)).FirstOrDefault();
        }
    }
}
