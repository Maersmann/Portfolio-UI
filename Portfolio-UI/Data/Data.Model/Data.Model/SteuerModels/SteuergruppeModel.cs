using Data.Types.SteuerTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.SteuerModels
{
    public class SteuergruppeModel
    {
        public int ID { get; set; }
        public SteuerHerkunftTyp SteuerHerkunftTyp { get; set; }
        public Double BetragVorZwischensumme { get; set; }
        public Double BetragNachZwischensumme { get; set; }

        public IList<SteuerModel> Steuern;
    }
}
