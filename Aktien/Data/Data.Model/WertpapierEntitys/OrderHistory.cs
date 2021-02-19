using Aktien.Data.Types;
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
    [Table("OrderHistory")]
    public class OrderHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }   
        public Double Anzahl { get; set; }
        public Double Preis { get; set; }
        public Double? Fremdkostenzuschlag { get; set; }
        public DateTime Orderdatum { get; set; }
        public int WertpapierID { get; set; }
        public Wertpapier Wertpapier { get; set; }

        [EnumDataType(typeof(KaufTypes))]
        public KaufTypes KaufartTyp { get; set; }

        [EnumDataType(typeof(OrderTypes))]
        public OrderTypes OrderartTyp { get; set; }

        [EnumDataType(typeof(OrderTypes))]
        public BuySell BuySell { get; set; }
        
    }
}
