using Aktien.Data.Model.AktieModels;
using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Data.Model.AktieModels
{
    [Table("OrderHistory")]
    public class OrderHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }   
        public int Anzahl { get; set; }
        public Double Preis { get; set; }
        public Double? Fremdkostenzuschlag { get; set; }
        public DateTime Orderdatum { get; set; }
        public int AktieID { get; set; }
        public Aktie Aktie { get; set; }

        [EnumDataType(typeof(KaufTypes))]
        public KaufTypes KaufartTyp { get; set; }

        [EnumDataType(typeof(OrderTypes))]
        public OrderTypes OrderartTyp { get; set; }

        [EnumDataType(typeof(OrderTypes))]
        public BuySell BuySell { get; set; }
        
    }
}
