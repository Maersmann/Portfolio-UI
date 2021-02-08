using Aktien.Data.Model.WertpapierModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Model.DepotModels
{
    [Table("DepotWertpapier")]
    public class DepotWertpapier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Double Anzahl { get; set; }
        public Double BuyIn { get; set; }
        public int WertpapierID { get; set; }
        public Wertpapier Wertpapier { get; set; }
        public int DepotID { get; set; }
        public Depot Depot { get; set; }
    }
}
