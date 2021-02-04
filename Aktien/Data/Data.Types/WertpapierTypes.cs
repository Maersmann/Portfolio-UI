using Aktien.Data.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Types
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum WertpapierTypes
    {
        [Description("Aktie")]
        Aktie = 0,
        [Description("ETF")]
        ETF = 1
    }
}
