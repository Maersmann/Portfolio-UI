using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Model.WertpapierEntitys
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
        public Double? BetragUmgerechnet { get; set; }

        [EnumDataType(typeof(DividendenRundungTypes))]
        public DividendenRundungTypes RundungArt { get; set; }
        [EnumDataType(typeof(Waehrungen))]
        public Waehrungen Waehrung { get; set; }


        [NotMapped]
        public Double Eurobetrag { get { var ret = Betrag; if (BetragUmgerechnet.HasValue) ret = BetragUmgerechnet.Value; return ret; } }
        [NotMapped]
        public Waehrungen EuroWaehrung { get { return Waehrungen.Euro; } }
    }
}
