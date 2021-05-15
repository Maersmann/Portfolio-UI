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
    public enum ProfitTypes
    {
        [Description("Thesaurierend")]
        Thesaurierend = 0,
        [Description("Ausschüttend")]
        Ausschuettend = 1,
    }
}
