using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Types.DepotTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum AusgabenArtTypes
    {
        [Description("Auszahlung")]
        Auszahlung = 0,
        [Description("Kauf")]
        Kauf = 1,
        [Description("Storno-Dividende")]
        StornoDividende = 2
    }
}
