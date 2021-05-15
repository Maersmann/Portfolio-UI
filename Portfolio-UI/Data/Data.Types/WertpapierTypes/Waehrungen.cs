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
    public enum Waehrungen
    {
        [Description("EUR")]
        Euro = 0,
        [Description("USD")]
        Dollar = 1,
        [Description("JPY")]
        JapanYen = 2
    }


}
