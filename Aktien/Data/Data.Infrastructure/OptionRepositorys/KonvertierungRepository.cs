using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.OptionEntitys;
using Aktien.Data.Types.OptionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aktien.Data.Infrastructure.OptionRepositorys
{
    public class KonvertierungRepository : BaseRepository
    {
        public void Speichern(KonvertierungTypes typ)
        {
            var Entity = new Konvertierung
            {
                Typ = typ
            };

            repo.Konvertierungen.Add(Entity);
            repo.SaveChanges();
        }

        public bool IstVorhanden(KonvertierungTypes typ)
        {
            var Aktie = repo.Konvertierungen.Where(k => k.Typ.Equals(typ)).FirstOrDefault();

            return (Aktie != null);
        }
    }
}
