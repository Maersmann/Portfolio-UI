using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Types.DividendenTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum DividendenRundungTypes
    {
        [Description("Standard")]
        Normal = 0,
        [Description("Aufrunden")]
        Up = 1,
        [Description("Abrunden")]
        Down = 2
    }
}
