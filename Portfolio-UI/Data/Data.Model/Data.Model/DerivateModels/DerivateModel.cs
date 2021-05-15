using Aktien.Data.Types.WertpapierTypes;
using Data.Model.WertpapierModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DerivateModels
{
    public class DerivateModel : WertpapierModel
    {
        public DerivateModel()
        {
            this.WertpapierTyp = WertpapierTypes.Derivate;
        }
    }
}
