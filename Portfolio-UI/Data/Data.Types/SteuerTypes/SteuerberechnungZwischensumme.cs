using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data.Types.SteuerTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SteuerberechnungZwischensumme
    {
        [Description("Vor der Zwischensumme")]
        vorZwischensumme = 0,
        [Description("Nach der Zwischensumme")]
        nachZischensumme = 1
    }
}
