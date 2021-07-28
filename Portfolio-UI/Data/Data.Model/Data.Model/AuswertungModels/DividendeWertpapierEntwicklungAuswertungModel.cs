using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels
{
    public class DividendeWertpapierEntwicklungAuswertungModel
    {
        public string Name { get; set; }
        public WertpapierTypes WertpapierTyp { get; set; }
        public string ISIN { get; set; }
        public string WKN { get; set; }
        public string WertpapierTypBez
        {
            get
            {
                switch (WertpapierTyp)
                {
                    case WertpapierTypes.Aktie: 
                        return "Aktie";
                    case WertpapierTypes.none:
                        return "";
                    case WertpapierTypes.ETF:
                        return "ETF";
                    case WertpapierTypes.Derivate:
                        return "Derivate";
                    default:
                        return "";
                }
            }
        }

        public IList<DividendeWertpapierEntwicklungAuswertungBetragModel> Betraege { get; set; }

        public DividendeWertpapierEntwicklungAuswertungModel()
        {
            WertpapierTyp = WertpapierTypes.none;
            Betraege = new List<DividendeWertpapierEntwicklungAuswertungBetragModel>();
        }
    }

    public class DividendeWertpapierEntwicklungAuswertungBetragModel
    {
        public DateTime Datum { get; set; }
        public double Betrag { get; set; }
    }
}
