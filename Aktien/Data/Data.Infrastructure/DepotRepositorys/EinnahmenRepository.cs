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
        public void Speichern(int? inID, Double inBetrag, DateTime inDatum, EinnahmeArtTypes inTyp, int inDepotID, int? inHerkunftID, string inBeschreibung)
        {
            var Entity = new Einnahme();

            if (inID.GetValueOrDefault(0).Equals(0))
                inID = null;

            if (inID.HasValue)
                Entity = repo.Einnahmen.Find(inID.Value);

            Entity.Art = inTyp;
            Entity.Betrag = Math.Round(inBetrag, 2, MidpointRounding.AwayFromZero);
            Entity.Datum = inDatum;
            Entity.DepotID = inDepotID;
            Entity.HerkunftID = inHerkunftID;
            Entity.Beschreibung = inBeschreibung;

            if (!inID.HasValue)
                repo.Einnahmen.Add(Entity);

            repo.SaveChanges();
        }

        public ObservableCollection<Einnahme> LoadAll()
        {
            return new ObservableCollection<Einnahme>(repo.Einnahmen.OrderByDescending(o => o.Datum).ToList());
        }

        public void Speichern(Einnahme inEinnahme)
        {
            if (inEinnahme.ID == 0)
                repo.Einnahmen.Add(inEinnahme);

            inEinnahme.Betrag = Math.Round(inEinnahme.Betrag, 2, MidpointRounding.AwayFromZero);

            repo.SaveChanges();
        }

        public void Entfernen( Einnahme inEinahme )
        {
            repo.Einnahmen.Remove(inEinahme);
            repo.SaveChanges();
        }

        public void EntferneAlle()
        {
            repo.Einnahmen.RemoveRange(repo.Einnahmen);
            repo.SaveChanges();
        }

        public Einnahme LoadByID(int inID)
        {
            return repo.Einnahmen.Find(inID);
        }

        public Einnahme LoadByHerkunftIDAndArt(int inHerkunftID, EinnahmeArtTypes inTyp)
        {
            return repo.Einnahmen.Where(e => e.HerkunftID.HasValue).Where(e => e.HerkunftID.Value.Equals(inHerkunftID)).Where(e => e.Art.Equals(inTyp)).FirstOrDefault();
        }
    }
}
