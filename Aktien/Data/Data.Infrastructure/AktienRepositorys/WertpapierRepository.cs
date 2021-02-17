using Aktien.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types.WertpapierTypes;
using Microsoft.EntityFrameworkCore;

namespace Aktien.Data.Infrastructure.AktienRepositorys
{
    public class WertpapierRepository: BaseRepository
    {

        public void Speichern(int? inID, String inName, String inISIN, String inWKN, WertpapierTypes inTyp)
        {
            var Entity = new Wertpapier();
            if (inID.HasValue)
                Entity = repo.Wertpapiere.Find(inID.Value);

            Entity.Name = inName;
            Entity.ISIN = inISIN;
            Entity.WKN = inWKN;
            Entity.WertpapierTyp = inTyp;

            if (!inID.HasValue)
                repo.Wertpapiere.Add(Entity);

            repo.SaveChanges();
        }

        public void Speichern(Wertpapier inWertpapier)
        {
            if (inWertpapier.ID == 0)
                repo.Wertpapiere.Add(inWertpapier);

            repo.SaveChanges();
        }

        public bool IstVorhanden( String inISIN )
        {
            var Aktie = repo.Wertpapiere.Where(a => a.ISIN.Equals(inISIN)).FirstOrDefault();

            return ( Aktie != null );
        }

        public ObservableCollection<Wertpapier> LadeAlle()
        {
            return new ObservableCollection<Wertpapier>(repo.Wertpapiere.OrderBy(o => o.ID).ToList());
        }

        public ObservableCollection<Wertpapier> LadeAlleByWertpapierTyp( WertpapierTypes inWertpapiertyp)
        {
            return new ObservableCollection<Wertpapier>(repo.Wertpapiere.Include(w => w.ETFInfo).Where(w => w.WertpapierTyp == inWertpapiertyp).OrderBy(o => o.ID).ToList());
        }

        public Wertpapier LadeAnhandID( int inID )
        {
            return repo.Wertpapiere.Include(w => w.ETFInfo).Where(a => a.ID == inID). First();
        }

        public void Entfernen( Wertpapier inWertpapier)
        {
            repo.Wertpapiere.Remove(inWertpapier);
            repo.SaveChanges();
        }

    }
}
