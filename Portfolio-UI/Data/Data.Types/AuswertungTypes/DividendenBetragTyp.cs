using Base.Logic.Types.Converter;
using System.ComponentModel;

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
