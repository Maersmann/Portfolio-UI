using Aktien.Data.Types.DividendenTypes;
using Data.Model.SteuerModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DividendeModels
{
    public class DividendeReitAktualisierungModel
    {
        public int DividendeErhaltenID { get; set; }
        public DividendenRundungTypes RundungArtAktualisierung { get; set; }
        public DividendenRundungTypes RundungArtStornierung { get; set; }
        public IList<SteuerModel> SteuernNeu { get; set; }
        public IList<SteuerModel> SteuernStorniert { get; set; }

        public DividendeReitAktualisierungModel()
        {
            SteuernNeu = new List<SteuerModel>();
            SteuernStorniert = new List<SteuerModel>();
            RundungArtStornierung = DividendenRundungTypes.Normal;
            RundungArtAktualisierung = DividendenRundungTypes.Normal;
        }
    }
}
