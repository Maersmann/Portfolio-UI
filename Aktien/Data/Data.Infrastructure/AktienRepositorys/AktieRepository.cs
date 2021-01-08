using Aktien.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.AktieModels;

namespace Aktien.Data.Infrastructure.Aktien.Repository
{
    public class AktieRepository: BaseRepository
    {

        public void Speichern(Aktie inAktie)
        {
            repo.Aktien.Add(inAktie);
            repo.SaveChanges();
        }

        public void Update(Aktie inAkite )
        {
            repo.Aktien.Update(inAkite);
            repo.SaveChanges();
        }

        public bool IstVorhanden( String inISIN )
        {
            var Aktie = repo.Aktien.Where(a => a.ISIN.Equals(inISIN)).FirstOrDefault();

            return ( Aktie != null );
        }

        public ObservableCollection<Aktie> LadeAlle()
        {
            return new ObservableCollection<Aktie>(repo.Aktien.OrderBy(o => o.ID).ToList());
        }

        public Aktie LadeAnhandID( int inID )
        {
            return repo.Aktien.Where(a => a.ID == inID).First();
        }

        public void Entfernen( Aktie inAktie )
        {
            repo.Aktien.Remove( inAktie );
            repo.SaveChanges();
        }

    }
}
