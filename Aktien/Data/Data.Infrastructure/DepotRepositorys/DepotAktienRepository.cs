using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.DepotModels;

namespace Aktien.Data.Infrastructure.Depots.Repository
{
    public class DepotAktienRepository : BaseRepository
    {
        public DepotAktie LadeAnhandAktieID(int AktieID)
        {
            return repo.AktienInDepots.Where(a => a.AktieID == AktieID).FirstOrDefault();
        }

        public Boolean IstAktieinDepotVorhanden( int AktieID )
        {
            return repo.AktienInDepots.Where(a => a.AktieID == AktieID).FirstOrDefault() != null;
        }

        public void Speichern(DepotAktie depotAktie)
        {
            if (depotAktie.ID != 0)
                repo.AktienInDepots.Update(depotAktie);
            else
                repo.AktienInDepots.Add(depotAktie);

            repo.SaveChanges();
        }
    }
}
