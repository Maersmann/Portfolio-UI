using Aktien.Data.Types.WertpapierTypes;
using Data.Types.SteuerTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.SteuerModels
{
    public class SteuerModel
    {
        public int ID { get; set; }
        public double Betrag { get; set; }
        public bool Optimierung { get; set; }
        public DateTime Datum { get; set; }
        public Waehrungen Waehrung { get; set; }
        public SteuerartModel Steuerart { get; set; }
        public int SteuergruppeID { get; set; }


        public SteuerModel DeepCopy()
        {
            SteuerModel othercopy = (SteuerModel)MemberwiseClone();
            return othercopy;
        }
    }
}
