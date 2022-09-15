using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data.Types.SparplanTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SparplanStatus
    {
        [Description("Aktiv")]
        aktiv = 0,
        [Description("Beendet")]
        beendet = 1
    }
}
