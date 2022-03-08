using Aktien.Data.Types;
using Base.Logic.ViewModels;
using Data.Model.DepotModels;
using Data.Types.ParamTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.UI.DepotViewModels
{
    public class OrderBuchViewModel : ViewModelUebersicht<OrderHistoryModel, StammdatenTypes>
    {
        private KaufTypes kaufTyp;
        private BuySell buySell;
        private OrderTypes orderTyp;
        private DateTime von;
        private DateTime bis;

        public OrderBuchViewModel()
        {
            kaufTyp = Data.Types.ParamTypes.KaufTypes.Alle;
            buySell = BuySell.Alle;
            orderTyp = Data.Types.ParamTypes.OrderTypes.Alle;
            bis = DateTime.Now;
            von = DateTime.Now.AddYears(-1);
            Title = "Orderbuch";
            _ = LoadData();
        }

        protected override bool LoadingOnCreate() => false;
        protected override string GetREST_API() 
        {
            string REST = $"/api/OrderHistory?von={von:MM/dd/yyyy}&bis={bis:MM/dd/yyyy}";
            if (kaufTyp != Data.Types.ParamTypes.KaufTypes.Alle)
            {
                REST += REST.Contains("?") ? $"&kauftyp={(int) kaufTyp}" : $"?kauftyp={(int) kaufTyp}";
            }

            if (orderTyp != Data.Types.ParamTypes.OrderTypes.Alle)
            {
                REST += REST.Contains("?") ? $"&ordertyp={(int)orderTyp}" : $"?ordertyp={(int)orderTyp}";
            }

            if (buySell != BuySell.Alle)
            {
                REST += REST.Contains("?") ? $"&buysell={(int)buySell}" : $"?buysell={(int)buySell}";
            }


            return REST;
        }
        protected override bool WithPagination() { return true; }


        #region Bindings
        public IEnumerable<KaufTypes> KaufTypes => Enum.GetValues(typeof(KaufTypes)).Cast<KaufTypes>();
        public IEnumerable<OrderTypes> OrderTypes => Enum.GetValues(typeof(OrderTypes)).Cast<OrderTypes>();
        public IEnumerable<BuySell> BuySellTypes => Enum.GetValues(typeof(BuySell)).Cast<BuySell>();
        public KaufTypes KaufTyp
        {
            get => kaufTyp;
            set
            {
                kaufTyp = value;
                RaisePropertyChanged();
            }
        }
        public OrderTypes OrderTyp
        {
            get => orderTyp;
            set
            {
                orderTyp = value;
                RaisePropertyChanged();
            }
        }
        public BuySell BuySell
        {
            get => buySell;
            set
            {
                buySell = value;
                RaisePropertyChanged();
            }
        }
        public DateTime? Von
        {
            get => von;
            set
            {
                if (!value.HasValue)
                { 
                    value = von;
                }
                if (RequestIsWorking || !Equals(von, value))
                {
                    von = value.Value;
                }
                RaisePropertyChanged();
            }
        }
        public DateTime? Bis
        {
            get =>bis;
            set
            {
                if (!value.HasValue)
                {
                    value = bis;
                }
                if (RequestIsWorking || !Equals(bis, value))
                {
                    bis = value.Value;       
                }
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}