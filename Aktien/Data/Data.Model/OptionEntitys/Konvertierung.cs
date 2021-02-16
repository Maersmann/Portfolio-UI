using Aktien.Data.Types.OptionTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aktien.Data.Model.OptionEntitys
{
    [Table("Konvertierung")]
    public class Konvertierung
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [EnumDataType(typeof(KonvertierungTypes))]
        public KonvertierungTypes Typ { get; set; }

    }
}
