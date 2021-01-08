using Aktien.Data.Model.AktieModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Model.DepotModels
{
    [Table("DepotAktien")]
    public class DepotAktie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Anzahl { get; set; }
        public Double BuyIn { get; set; }
        public int AktieID { get; set; }
        public Aktie Aktie { get; set; }
        public int DepotID { get; set; }
        public Depot Depot { get; set; }
    }
}
