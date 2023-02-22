using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data.Types.AuswertungTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ZinsenBetragTyp
    {
        [Description("Gesamt")]
        Gesamt = 0,
        [Description("Erhalten")]
        Erhalten = 1
    }
}
