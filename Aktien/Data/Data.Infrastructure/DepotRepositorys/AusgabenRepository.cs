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
    public class AusgabenRepository : BaseRepository
    {
        public void Speichern(int? iD, Double betrag, DateTime datum, AusgabenArtTypes typ, int depotID, int? herkunftID, string beschreibung)
        {
            var Entity = new Ausgabe();

            if (iD.GetValueOrDefault(0).Equals(0))
                iD = null;

            if (iD.HasValue)
                Entity = repo.Ausgaben.Find(iD.Value);

            Entity.Art = typ;
            Entity.Betrag = Math.Round(betrag, 2, MidpointRounding.AwayFromZero);
            Entity.Datum = datum;
            Entity.DepotID = depotID;
            Entity.HerkunftID = herkunftID;
            Entity.Beschreibung = beschreibung;

            if (!iD.HasValue)
                repo.Ausgaben.Add(Entity);

            repo.SaveChanges();
        }

        public ObservableCollection<Ausgabe> LoadAll()
        {
            return new ObservableCollection<Ausgabe>(repo.Ausgaben.OrderByDescending(o => o.Datum).ToList());
        }

        public void Speichern(Ausgabe ausgabe)
        {
            if (ausgabe.ID == 0)
                repo.Ausgaben.Add(ausgabe);

            ausgabe.Betrag = Math.Round(ausgabe.Betrag, 2, MidpointRounding.AwayFromZero);

            repo.SaveChanges();
        }

        public void Entfernen(Ausgabe ausgabe)
        {
            repo.Ausgaben.Remove(ausgabe);
            repo.SaveChanges();
        }

        public void EntferneAlle()
        {
            repo.Ausgaben.RemoveRange(repo.Ausgaben);
            repo.SaveChanges();
        }

        public Ausgabe LoadByID(int id)
        {
            return repo.Ausgaben.Find(id);
        }

        public Ausgabe LoadByHerkunftIDAndArt(int herkunftID, AusgabenArtTypes typ)
        {
            return repo.Ausgaben.Where(e => e.HerkunftID.HasValue).Where(e => e.HerkunftID.Value.Equals(herkunftID)).Where(e => e.Art.Equals(typ)).FirstOrDefault();
        }
    }
}
