using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data.Types.DividendenTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum DividendeErhaltenArt
    {
        [Description("Normal")]
        normal = 0,
        [Description("Storniert")]
        storniert = 1,
        [Description("Aktualisiert")]
        aktualisiert = 2
    }
}
