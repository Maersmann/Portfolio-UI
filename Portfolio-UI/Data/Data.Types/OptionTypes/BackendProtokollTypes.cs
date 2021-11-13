using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data.Types.OptionTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum BackendProtokollTypes
    {
        [Description("HTTP")]
        http = 0,
        [Description("HTTPS")]
        https = 1
    }
}
