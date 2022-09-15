using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data.Types.SparplanTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SparplanIntervall
    {
        [Description("Zweimal im Monat")]
        zweiMalImMonat = 0,
        [Description("Monatlich")]
        monatlich = 1,
        [Description("Quartalsweise")]
        quartalsweise = 2,
    }
}
