using Aktien.Data.Model.DepotEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.OptionenViewModels.Models
{
    public class WertpapierBuyInModel
    {
        public string WertpapierName { get; set; }
        public double AlterBuyIn { get; set; }
        public double NeuerBuyIn { get; set; }
        public double DepotWertpapierID { get; set; }

        public DepotWertpapier DepotWertpapier { get; set; }
    }
}
