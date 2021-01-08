using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.DepotModels;
using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aktien.Data.Infrastructure.Depots.Repository
{
    public class OrderHistoryRepository : BaseRepository
    {
        public void Speichern(Double inPreis, Double? inFremdkosten, DateTime inDatum, int inAktieID, int inAnzahl, KaufTypes inKauftyp, OrderTypes inOrderTyp)
        {
            repo.OrderHistories.Add(new OrderHistory { AktieID = inAktieID, Preis = inPreis, Kaufdatum = inDatum, Anzahl = inAnzahl, Fremdkostenzuschlag = inFremdkosten, KaufartTyp = inKauftyp, OrderartTyp = inOrderTyp, BuySell = BuySell.Buy });
            repo.SaveChanges();
        }
    }
}
