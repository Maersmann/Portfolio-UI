using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.ZinsenModels
{
    public class ZinsenErhaltenUebersichtModel
    {
        public int ID { get; set; }
        public DateTime ErhaltenAm { get; set; }
        public Double Erhalten { get; set; }
        public Double Prozent { get; set; }
        public Double DurchschnittlicherKontostand { get; set; }
        public Double Steuern { get; set; }
        public Double Gesamt { get; set; }
    }
}
