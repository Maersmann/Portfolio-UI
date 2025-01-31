using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Types.WertpapierTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum WertpapierTypes
    {
        none = -1,
        [Description("Aktie")]
        Aktie = 0,
        [Description("ETF")]
        ETF = 1,
        [Description("Derivate")]
        Derivate = 2,
        [Description("Crypto")]
        Crypto = 3
    }
}
