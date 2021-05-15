using Data.Model.DepotModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.WertpapierModels
{
    public class ReverseSplitEintragenModel
    {
        public DepotWertpapierModel AltWertpapier;
        public DepotWertpapierModel NeuWertpapier;

        public ReverseSplitEintragenModel()
        {
            AltWertpapier = new DepotWertpapierModel();
            NeuWertpapier = new DepotWertpapierModel();
        }
    }
}
