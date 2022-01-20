using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.AuswertungModels.SteueModels
{
    public class SteuerMonatgesamtbetragAuswertungModel
    {
        public double Betrag { get; set; }
        public int Jahr { get; set; }
        public IList<SteuerArtMonatgesamtbetragAuswertungModel> Arten { get; set; }

        public SteuerMonatgesamtbetragAuswertungModel()
        {
            Arten = new List<SteuerArtMonatgesamtbetragAuswertungModel>();
            Betrag = 0;
        }

    }

    public class SteuerArtMonatgesamtbetragAuswertungModel
    {
        public string Bezeichnung { get; set; }
        public double Betrag { get; set; }
        public SteuerArtMonatgesamtbetragAuswertungModel()
        {
            Betrag = 0;
        }
    }
}
