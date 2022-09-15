using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO.WertpapierDTOs
{
    public class WertpapierDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ISIN { get; set; }
        public string WKN { get; set; }
        public WertpapierTypes WertpapierTyp { get; set; }
    }
}
