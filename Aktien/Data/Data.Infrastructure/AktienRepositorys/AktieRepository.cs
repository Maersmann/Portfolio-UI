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

        public void Speichern(int? inID, String inName, String inISIN, String inWKN)
        {
            var Entity = new Aktie();
            if (inID.HasValue)
                Entity = repo.Aktien.Find(inID.Value);

            Entity.Name = inName;
            Entity.ISIN = inISIN;
            Entity.WKN = inWKN;

            if (!inID.HasValue)
                repo.Aktien.Add(Entity);

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
