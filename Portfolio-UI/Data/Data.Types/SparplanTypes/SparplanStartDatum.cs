using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data.Types.SparplanTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SparplanStartDatum
    {
        [Description("Anfang des Monats")]
        anfangDesMonats = 0,
        [Description("Mitte des Monats")]
        mitteDesMonats = 1,
    }
}
