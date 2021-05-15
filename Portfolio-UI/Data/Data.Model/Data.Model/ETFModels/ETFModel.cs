using Aktien.Data.Types.WertpapierTypes;
using Data.Model.WertpapierModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.ETFModels
{
    public class ETFModel : WertpapierModel
    {
        public ETFModel()
        {
            this.WertpapierTyp = WertpapierTypes.ETF;
        }

        public ProfitTypes ProfitTyp { get; set; }
        public ErmittentTypes Emittent { get; set; }
        public int ETFInfoID { get; set; }


    }
}
