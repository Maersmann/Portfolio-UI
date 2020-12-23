using Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Data.API
{
    public class AktieAPI
    {
        private readonly RepositoryBase repo;
        public AktieAPI()
        {
            repo = GlobalVariables.GetRepoBase();
        }

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

        public bool IstAkieVorhanden( String inISIN )
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

    }
}
