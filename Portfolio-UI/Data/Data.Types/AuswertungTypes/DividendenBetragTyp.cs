using Base.Logic.Types.Converter;
using System.ComponentModel;

namespace Data.Types.AuswertungTypes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum DividendenBetragTyp
    {
        [Description("Netto")]
        Netto = 0,
        [Description("Brutto")]
        Brutto = 1
    }
}
