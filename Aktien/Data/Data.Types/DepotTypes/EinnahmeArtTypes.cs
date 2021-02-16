using Aktien.Data.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Types.DepotTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EinnahmeArtTypes
    {
        [Description("Einzahlung")]
        Einzahlung = 0,
        [Description("Dividende")]
        Dividende = 1,
        [Description("Verkauf")]
        Verkauf = 2
    }
}
