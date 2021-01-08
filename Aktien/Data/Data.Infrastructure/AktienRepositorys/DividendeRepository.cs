using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.AktieModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Aktien.Data.Infrastructure.Aktien.Repository
{
    public class DividendeRepository : BaseRepository
    {
        public void Speichern( Double inBetrag, DateTime inDatum, int inAktieID )
        {
            repo.Dividenden.Add(new Dividende { AktieID = inAktieID, Betrag = inBetrag, Datum = inDatum });
            repo.SaveChanges();
        }
        public void Update( Double inBetrag, DateTime inDatum, int inID )
        {
            var dividende = repo.Dividenden.Find(inID);
            dividende.Betrag = inBetrag;
            dividende.Datum = inDatum;
            repo.SaveChanges();
        }

        public ObservableCollection<Dividende> LadeAlleFuerAktie( int inAktieID )
        {
            return new ObservableCollection<Dividende>(repo.Dividenden.Where(d=>d.AktieID == inAktieID).OrderBy( d=>d.Datum ).ToList());
        }

        public Dividende LadeAnhandID(int inID)
        {
            return repo.Dividenden.Where(a => a.ID == inID).First();
        }

        public void Entfernen(int inID)
        {
            repo.Dividenden.Remove( repo.Dividenden.Find( inID ) );
            repo.SaveChanges();
        }
    }
}
