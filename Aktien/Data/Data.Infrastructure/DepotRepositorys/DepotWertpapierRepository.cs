using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.DepotEntitys;
using System.Collections.ObjectModel;

namespace Aktien.Data.Infrastructure.DepotRepositorys
{
    public class DepotWertpapierRepository : BaseRepository
    {
        public DepotWertpapier LadeByWertpapierID(int inWertpapierID)
        {
            return repo.AktienInDepots.Where(a => a.WertpapierID == inWertpapierID).FirstOrDefault();
        }

        public Boolean IstWertpapierInDepotVorhanden( int inWertpapierID)
        {
            return repo.AktienInDepots.Where(a => a.WertpapierID == inWertpapierID).FirstOrDefault() != null;
        }

        public void Speichern(int? inID, Double inAnzahl, Double inBuyIn, int inWertpapierID, int inDepotID)
        {
            var Entity = new DepotWertpapier();

            if (inID.GetValueOrDefault(0).Equals(0))
                inID = null;

            if (inID.HasValue)
                Entity = repo.AktienInDepots.Find(inID.Value);

            Entity.BuyIn = Math.Round(inBuyIn, 3, MidpointRounding.AwayFromZero);

            Entity.Anzahl = inAnzahl;
            Entity.WertpapierID = inWertpapierID;
            Entity.DepotID = inDepotID;

            if (!inID.HasValue)
                repo.AktienInDepots.Add(Entity);

            repo.SaveChanges();
        }

        public ObservableCollection<DepotWertpapier> LoadAll()
        {
            return new ObservableCollection<DepotWertpapier>(repo.AktienInDepots.OrderBy(o => o.ID).ToList());
        }

        public void Entfernen(DepotWertpapier depotAktie)
        {
            repo.AktienInDepots.Remove(depotAktie);
            repo.SaveChanges();
        }
    }
}
