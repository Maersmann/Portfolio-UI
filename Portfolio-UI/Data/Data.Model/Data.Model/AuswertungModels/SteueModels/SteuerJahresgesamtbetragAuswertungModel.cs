using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels.SteueModels
{
    public class SteuerJahresgesamtbetragAuswertungModel
    {
        public double Betrag { get; set; }
        public int Jahr { get; set; }
        public IList<SteuerArtJahresgesamtbetragAuswertungModel> Arten { get; set; }

        public SteuerJahresgesamtbetragAuswertungModel()
        {
            Arten = new List<SteuerArtJahresgesamtbetragAuswertungModel>();
            Betrag = 0;
        }

    }

    public class SteuerArtJahresgesamtbetragAuswertungModel
    {
        public string Bezeichnung { get; set; }
        public double Betrag { get; set; }
        public SteuerArtJahresgesamtbetragAuswertungModel()
        {
            Betrag = 0;
        }
    }
}
