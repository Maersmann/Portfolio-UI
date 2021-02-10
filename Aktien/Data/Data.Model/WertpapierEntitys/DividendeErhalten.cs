using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aktien.Data.Model.WertpapierEntitys
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
        public Double Bestand { get; set; }
        public int DividendeID { get; set; }
        public Dividende Dividende { get; set; }
        public int WertpapierID { get; set; }
        public Wertpapier Wertpapier { get; set; }

    }
}
