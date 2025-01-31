
using Aktien.Data.Types.WertpapierTypes;
using Data.Model.WertpapierModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.CryptoModels
{
    public class CryptoModel : WertpapierModel
    {
        public CryptoModel()
        {
            this.WertpapierTyp = WertpapierTypes.Crypto;
        }
    }
}
