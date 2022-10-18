using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data.Types.SparplanTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SparplanHistoryArt
    {
        [Description("Ausgeführt")]
        ausgefuehrt = 0,
        [Description("Fehlgeschlagen")]
        fehlgeschlagen = 1
    }
}
