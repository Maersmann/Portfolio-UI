using Aktien.Data.Types.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data.Types.AuswertungTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum DividendenBetragTyp
    {
        [Description("Nach Steuer")]
        NachSteuer = 0,
        [Description("Vor Steuer")]
        VorSteuer = 1
    }
}
