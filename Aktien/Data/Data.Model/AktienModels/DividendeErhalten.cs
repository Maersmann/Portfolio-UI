using Aktien.Data.Model.AktienModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aktien.Data.Model.AktienModels
{
    [Table("DividendeErhalten")]
    public class DividendeErhalten
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime Datum { get; set; }
        public Double? Quellensteuer { get; set; }
        public Double? Umrechnungskurs { get; set; }
        public Double GesamtNetto { get; set; }
        public Double GesamtBrutto { get; set; }
        public int Bestand { get; set; }
        public int AktieID { get; set; }
        public Aktie Aktie { get; set; }
        public int DividendeID { get; set; }
        public Dividende Dividende { get; set; }
    }
}
