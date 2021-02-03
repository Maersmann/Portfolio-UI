using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.DepotModels;
using System.Collections.ObjectModel;

namespace Aktien.Data.Infrastructure.DepotRepositorys
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

        public void Speichern(int? inID, Double inAnzahl, Double inBuyIn, int inAktieID, int inDepotID)
        {
            var Entity = new DepotAktie();

            if (inID.GetValueOrDefault(0).Equals(0))
                inID = null;

            if (inID.HasValue)
                Entity = repo.AktienInDepots.Find(inID.Value);

            Entity.BuyIn = inBuyIn;
            Entity.Anzahl = inAnzahl;
            Entity.AktieID = inAktieID;
            Entity.DepotID = inDepotID;

            if (!inID.HasValue)
                repo.AktienInDepots.Add(Entity);

            repo.SaveChanges();
        }

        public ObservableCollection<DepotAktie> LoadAll()
        {
            return new ObservableCollection<DepotAktie>(repo.AktienInDepots.OrderBy(o => o.ID).ToList());
        }

        public void Entfernen(DepotAktie depotAktie)
        {
            repo.AktienInDepots.Remove(depotAktie);
            repo.SaveChanges();
        }
    }
}
