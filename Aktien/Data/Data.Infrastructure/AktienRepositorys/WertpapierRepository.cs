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

        public void Speichern(int? iD, String name, String isin, String wkn, WertpapierTypes typ)
        {
            var Entity = new Wertpapier();
            if (iD.HasValue)
                Entity = repo.Wertpapiere.Find(iD.Value);

            Entity.Name = name;
            Entity.ISIN = isin;
            Entity.WKN = wkn;
            Entity.WertpapierTyp = typ;

            if (!iD.HasValue)
                repo.Wertpapiere.Add(Entity);

            repo.SaveChanges();
        }

        public void Speichern(Wertpapier wertpapier)
        {
            if (wertpapier.ID == 0)
                repo.Wertpapiere.Add(wertpapier);

            repo.SaveChanges();
        }

        public bool IstVorhanden( String isin )
        {
            var Aktie = repo.Wertpapiere.Where(a => a.ISIN.Equals(isin)).FirstOrDefault();

            return ( Aktie != null );
        }

        public ObservableCollection<Wertpapier> LadeAlle()
        {
            return new ObservableCollection<Wertpapier>(repo.Wertpapiere.OrderBy(o => o.ID).ToList());
        }

        public ObservableCollection<Wertpapier> LadeAlleByWertpapierTyp( WertpapierTypes wertpapiertyp)
        {
            return new ObservableCollection<Wertpapier>(repo.Wertpapiere.Include(w => w.ETFInfo).Where(w => w.WertpapierTyp == wertpapiertyp).OrderBy(o => o.ID).ToList());
        }

        public Wertpapier LadeAnhandID( int iD )
        {
            return repo.Wertpapiere.Include(w => w.ETFInfo).Where(a => a.ID == iD).FirstOrDefault();
        }

        public void Entfernen( Wertpapier wertpapier)
        {
            repo.Wertpapiere.Remove(wertpapier);
            repo.SaveChanges();
        }

    }
}
