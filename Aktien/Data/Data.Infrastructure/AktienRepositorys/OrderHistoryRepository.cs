using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.AktieModels;
using Aktien.Data.Model.DepotModels;
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
        public void Speichern(Double inPreis, Double? inFremdkosten, DateTime inDatum, int inAktieID, int inAnzahl, KaufTypes inKauftyp, OrderTypes inOrderTyp)
        {
            repo.OrderHistories.Add(new OrderHistory { AktieID = inAktieID, Preis = inPreis, Orderdatum = inDatum, Anzahl = inAnzahl, Fremdkostenzuschlag = inFremdkosten, KaufartTyp = inKauftyp, OrderartTyp = inOrderTyp, BuySell = BuySell.Buy });
            repo.SaveChanges();
        }

        public ObservableCollection<OrderHistory> LadeAlleByAktieID(int inAktieID)
        {
            return new ObservableCollection<OrderHistory>(repo.OrderHistories.Where(o => o.AktieID == inAktieID).OrderBy(o => o.ID).ToList());
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
