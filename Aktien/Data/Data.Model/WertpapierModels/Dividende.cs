using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Model.WertpapierModels
{
    [Table("Dividende")]
    public class Dividende
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int ID { get; set; }

        public DateTime Datum { get; set; }

        public Double Betrag { get; set; }

        public int WertpapierID { get; set; }
        public Wertpapier Wertpapier { get; set; }

        [EnumDataType(typeof(Waehrungen))]
        public Waehrungen Waehrung { get; set; }
        public Double? BetragUmgerechnet { get; set; }
    }
}
