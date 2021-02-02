using Aktien.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.AktienModels;

namespace Aktien.Data.Infrastructure.AktienRepositorys
{
    public class AktieRepository: BaseRepository
    {

        public void Speichern(Aktie inAktie)
        {
            repo.Aktien.Add(inAktie);
            repo.SaveChanges();
        }

        public void Update(Aktie inAktie)
        {
            repo.Aktien.Update(inAktie);
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
