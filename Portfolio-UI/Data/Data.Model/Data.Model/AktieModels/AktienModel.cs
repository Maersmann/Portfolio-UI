using Aktien.Data.Types.WertpapierTypes;
using Data.Model.WertpapierModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AktieModels
{
    public class AktienModel : WertpapierModel
    {
        public AktienModel()
        {
            this.WertpapierTyp = WertpapierTypes.Aktie;
        }
    }
}
