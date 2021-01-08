using Aktien.Data.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Types
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum KaufTypes
    {
        [Description("Kauf")]
        Kauf = 0,
        [Description("Spin-Off")]
        SpinOff = 1,
        [Description("Kapitalerhöhung")]
        Kapitalerhoehung = 2
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum OrderTypes
    {
        [Description("Order")]
        Normal = 0,
        [Description("Limi-Order")]
        Limit = 1,
        [Description("Stop-Order")]
        Stop = 2
    }

    public enum BuySell
    {
        Buy = 0,
        Sell = 1
    }
}
