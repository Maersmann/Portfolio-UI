using Aktien.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.WertpapierModels;
using Aktien.Data.Types;

namespace Aktien.Data.Infrastructure.AktienRepositorys
{
    public class WertpapierRepository: BaseRepository
    {

        public void Speichern(int? inID, String inName, String inISIN, String inWKN, WertpapierTypes inTyp)
        {
            var Entity = new Wertpapier();
            if (inID.HasValue)
                Entity = repo.Aktien.Find(inID.Value);

            Entity.Name = inName;
            Entity.ISIN = inISIN;
            Entity.WKN = inWKN;
            Entity.WertpapierTyp = inTyp;

            if (!inID.HasValue)
                repo.Aktien.Add(Entity);

            repo.SaveChanges();
        }

        public bool IstVorhanden( String inISIN )
        {
            var Aktie = repo.Aktien.Where(a => a.ISIN.Equals(inISIN)).FirstOrDefault();

            return ( Aktie != null );
        }

        public ObservableCollection<Wertpapier> LadeAlle()
        {
            return new ObservableCollection<Wertpapier>(repo.Aktien.OrderBy(o => o.ID).ToList());
        }

        public ObservableCollection<Wertpapier> LadeAlleByWertpapierTyp( WertpapierTypes inWertpapiertyp)
        {
            return new ObservableCollection<Wertpapier>(repo.Aktien.Where(w => w.WertpapierTyp == inWertpapiertyp).OrderBy(o => o.ID).ToList());
        }

        public Wertpapier LadeAnhandID( int inID )
        {
            return repo.Aktien.Where(a => a.ID == inID).First();
        }

        public void Entfernen( Wertpapier inWertpapier)
        {
            repo.Aktien.Remove(inWertpapier);
            repo.SaveChanges();
        }

    }
}
