using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entity.AktieEntitys
{
    [Table("Aktie")]
    public class Aktie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string ISIN { get; set; }

        public string WKN { get; set; }

        public List<Dividende> Dividenden { get; set; }
    }
}
