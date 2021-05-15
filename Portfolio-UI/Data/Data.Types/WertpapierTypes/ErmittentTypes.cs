using Aktien.Data.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Types.WertpapierTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ErmittentTypes
    {
        [Description("iShares")]
        iShares = 0,
        [Description("Amundi")]
        Amundi = 1,
        [Description("DWS")]
        DWS = 2,
        [Description("Lyxor")]
        Lyxor = 3,
        [Description("WisdomTree")]
        WisdomTree = 4,
    }
}
