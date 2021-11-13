using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels
{
    public class DividendeWertpapierEntwicklungMonatlichModel
    {
        public string Name { get; set; }
        public WertpapierTypes WertpapierTyp { get; set; }
        public string ISIN { get; set; }
        public string WKN { get; set; }
        public string WertpapierTypBez
        {
            get
            {
                return WertpapierTyp switch
                {
                    WertpapierTypes.Aktie => "Aktie",
                    WertpapierTypes.none => "",
                    WertpapierTypes.ETF => "ETF",
                    WertpapierTypes.Derivate => "Derivate",
                    _ => "",
                };
            }
        }

        public IList<DividendeWertpapierEntwicklungAuswertungBetragModel> Betraege { get; set; }

        public DividendeWertpapierEntwicklungMonatlichModel()
        {
            WertpapierTyp = WertpapierTypes.none;
            Betraege = new List<DividendeWertpapierEntwicklungAuswertungBetragModel>();
        }
    }

    public class DividendeWertpapierEntwicklungAuswertungBetragModel
    {
        public DateTime Datum { get; set; }
        public double Netto { get; set; }
        public double Brutto { get; set; }
    }
}
