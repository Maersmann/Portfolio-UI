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
        public void Speichern(int? inID, Double inBetrag, DateTime inDatum, AusgabenArtTypes inTyp, int inDepotID, int? inHerkunftID, string inBeschreibung)
        {
            var Entity = new Ausgabe();

            if (inID.GetValueOrDefault(0).Equals(0))
                inID = null;

            if (inID.HasValue)
                Entity = repo.Ausgaben.Find(inID.Value);

            Entity.Art = inTyp;
            Entity.Betrag = Math.Round(inBetrag, 4, MidpointRounding.AwayFromZero);
            Entity.Datum = inDatum;
            Entity.DepotID = inDepotID;
            Entity.HerkunftID = inHerkunftID;
            Entity.Beschreibung = inBeschreibung;

            if (!inID.HasValue)
                repo.Ausgaben.Add(Entity);

            repo.SaveChanges();
        }

        public ObservableCollection<Ausgabe> LoadAll()
        {
            return new ObservableCollection<Ausgabe>(repo.Ausgaben.OrderByDescending(o => o.Datum).ToList());
        }

        public void Speichern(Ausgabe inAusgabe)
        {
            if (inAusgabe.ID == 0)
                repo.Ausgaben.Add(inAusgabe);

            inAusgabe.Betrag = Math.Round(inAusgabe.Betrag, 4, MidpointRounding.AwayFromZero);

            repo.SaveChanges();
        }

        public void Entfernen(Ausgabe inAusgabe)
        {
            repo.Ausgaben.Remove(inAusgabe);
            repo.SaveChanges();
        }

        public Ausgabe LoadByID(int inID)
        {
            return repo.Ausgaben.Find(inID);
        }

        public Ausgabe LoadByHerkunftIDAndArt(int inHerkunftID, AusgabenArtTypes inTyp)
        {
            return repo.Ausgaben.Where(e => e.HerkunftID.HasValue).Where(e => e.HerkunftID.Value.Equals(inHerkunftID)).Where(e => e.Art.Equals(inTyp)).FirstOrDefault();
        }
    }
}
