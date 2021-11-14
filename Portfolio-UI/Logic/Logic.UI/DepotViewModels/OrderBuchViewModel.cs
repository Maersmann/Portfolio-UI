using Aktien.Data.Types;
using Base.Logic.ViewModels;
using Data.Model.DepotModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.UI.DepotViewModels
{
    public class OrderBuchViewModel : ViewModelUebersicht<OrderHistoryModel, StammdatenTypes>
    {
        public OrderBuchViewModel()
        {
            Title = "Orderbuch";
        }

        protected override string GetREST_API() { return $"/api/OrderHistory"; }
        protected override bool WithPagination() { return true; }
    }
}