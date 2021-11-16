using Base.Logic.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data.Types.ParamTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum KaufTypes
    {
        [Description("Alle")]
        Alle = -1,
        [Description("Normal")]
        Kauf = 0,
        [Description("Spin-Off")]
        SpinOff = 1,
        [Description("Kapitalerhöhung")]
        Kapitalerhoehung = 2,
        [Description("Ausbuchung")]
        Ausbuchung = 3,
        [Description("Einbuchung")]
        Einbuchung = 4
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum OrderTypes
    {
        [Description("Alle")]
        Alle = -1,
        [Description("Order")]
        Normal = 0,
        [Description("Limit-Order")]
        Limit = 1,
        [Description("Stop-Order")]
        Stop = 2,
        [Description("Sparplan")]
        Sparplan = 3,
        [Description("Reverse-Split")]
        ReverseSplit = 4,

    }

    public enum BuySell
    {
        [Description("Alle")]
        Alle = -1,
        Buy = 0,
        Sell = 1
    }
}
