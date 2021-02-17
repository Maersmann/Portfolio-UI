using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Aktien.Data.Infrastructure.AktienRepositorys
{
    public class OrderHistoryRepository : BaseRepository
    {
        public void Speichern(Double inPreis, Double? inFremdkosten, DateTime inDatum, int inWertpapierID, Double inAnzahl, KaufTypes inKauftyp, OrderTypes inOrderTyp, BuySell inBuySell)
        {
            repo.OrderHistories.Add(new OrderHistory { WertpapierID = inWertpapierID, Preis = inPreis, Orderdatum = inDatum, Anzahl = inAnzahl, Fremdkostenzuschlag = inFremdkosten, KaufartTyp = inKauftyp, OrderartTyp = inOrderTyp, BuySell = inBuySell });
            repo.SaveChanges();
        }
        public void Speichern(OrderHistory inHistory)
        {
            repo.OrderHistories.Add(inHistory);
            repo.SaveChanges();
        }

        public ObservableCollection<OrderHistory> LadeAlleByWertpapierID(int inWertpapierID)
        {
            return new ObservableCollection<OrderHistory>(repo.OrderHistories.Where(o => o.WertpapierID == inWertpapierID).OrderByDescending(o => o.ID ).ToList());
        }

        public OrderHistory LadeByID(int inID)
        {
            return repo.OrderHistories.Where(o => o.ID.Equals(inID)).FirstOrDefault();
        }

        public void Entfernen(OrderHistory order)
        {
            repo.OrderHistories.Remove(order);
            repo.SaveChanges();
        }

        public ObservableCollection<OrderHistory> LadeAlle()
        {
            return new ObservableCollection<OrderHistory>(repo.OrderHistories.OrderBy(o => o.ID).ToList());
        }

        public bool IstNeuereOrderVorhanden(int inWertpapierID, DateTime inDatum)
        {
            return repo.OrderHistories.Where(o => o.WertpapierID == inWertpapierID).Where(o => o.Orderdatum > inDatum).FirstOrDefault() != null;
        }
    }
}
