using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aktien.Data.Model.WertpapierEntitys
{
    [Table("ETFInfo")]
    public class ETFInfo
    {  
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int WertpapierID { get; set; }
        public Wertpapier Wertpapier { get; set; }

        [EnumDataType(typeof(ErmittentTypes))]
        public ErmittentTypes Emittent { get; set; }

        [EnumDataType(typeof(ProfitTypes))]
        public ProfitTypes ProfitTyp { get; set; }

    }
}
