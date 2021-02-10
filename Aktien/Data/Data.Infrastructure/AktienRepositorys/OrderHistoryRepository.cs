using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types;
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

        public ObservableCollection<OrderHistory> LadeAlleByWertpapierID(int inWertpapierID)
        {
            return new ObservableCollection<OrderHistory>(repo.OrderHistories.Where(o => o.WertpapierID == inWertpapierID).OrderBy(o => o.ID).ToList());
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
    }
}
