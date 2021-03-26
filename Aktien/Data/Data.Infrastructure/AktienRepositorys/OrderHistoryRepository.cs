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
        public void Speichern(Double preis, Double? fremdkosten, DateTime datum, int wertpapierID, Double anzahl, KaufTypes kauftyp, OrderTypes orderTyp, BuySell buySell)
        {
            repo.OrderHistories.Add(new OrderHistory { WertpapierID = wertpapierID, Preis = preis, Orderdatum = datum, Anzahl = anzahl, Fremdkostenzuschlag = fremdkosten, KaufartTyp = kauftyp, OrderartTyp = orderTyp, BuySell = buySell });
            repo.SaveChanges();
        }
        public void Speichern(OrderHistory orderhistory)
        {
            repo.OrderHistories.Add(orderhistory);
            repo.SaveChanges();
        }

        public ObservableCollection<OrderHistory> LadeAlleByWertpapierID(int wertpapierID)
        {
            return new ObservableCollection<OrderHistory>(repo.OrderHistories.Where(o => o.WertpapierID == wertpapierID).OrderByDescending(o => o.Orderdatum ).ToList());
        }

        public OrderHistory LadeByID(int iD)
        {
            return repo.OrderHistories.Where(o => o.ID.Equals(iD)).FirstOrDefault();
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

        public bool IstNeuereOrderVorhanden(int wertpapierID, DateTime datum)
        {
            return repo.OrderHistories.Where(o => o.WertpapierID == wertpapierID).Where(o => o.Orderdatum > datum).FirstOrDefault() != null;
        }
    }
}
