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
        public DepotWertpapier LadeByWertpapierID(int wertpapierID)
        {
            return repo.AktienInDepots.Where(a => a.WertpapierID == wertpapierID).FirstOrDefault();
        }

        public Boolean IstWertpapierInDepotVorhanden( int wertpapierID)
        {
            return repo.AktienInDepots.Where(a => a.WertpapierID == wertpapierID).FirstOrDefault() != null;
        }

        public void Speichern(int? id, Double anzahl, Double buyIn, int wertpapierID, int depotID)
        {
            var Entity = new DepotWertpapier();

            if (id.GetValueOrDefault(0).Equals(0))
                id = null;

            if (id.HasValue)
                Entity = repo.AktienInDepots.Find(id.Value);

            Entity.BuyIn = Math.Round(buyIn, 3, MidpointRounding.AwayFromZero);

            Entity.Anzahl = anzahl;
            Entity.WertpapierID = wertpapierID;
            Entity.DepotID = depotID;

            if (!id.HasValue)
                repo.AktienInDepots.Add(Entity);

            repo.SaveChanges();
        }
        public void Speichern(DepotWertpapier wertpapier)
        {
            if (wertpapier.ID == 0)
                repo.AktienInDepots.Add(wertpapier);

            wertpapier.BuyIn = Math.Round(wertpapier.BuyIn, 4, MidpointRounding.AwayFromZero);

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
