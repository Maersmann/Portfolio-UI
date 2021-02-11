using Aktien.Data.Types.DepotTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aktien.Data.Model.DepotEntitys
{
    [Table("Einnahme")]
    public class Einnahme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Double Betrag { get; set; }
        public DateTime Datum { get; set; }
        public EinnahmeArtTypes Art { get; set; }
        public int? HerkunftID { get; set; }
        public int DepotID { get; set; }
        public Depot Depot { get; set; }
    }
}
