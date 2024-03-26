using Data.Model.SteuerModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO.ZinsenDTOs
{
    public class ZinsenEintragenDTO
    {
        public int ID { get; set; }
        public DateTime ErhaltenAm { get; set; }
        public DateTime Abrechnungsmonat { get; set; }

        public double Gesamt { get; set; }
        public double Prozent { get; set; }
        public double DurchschnittlicherKontostand { get; set; }
        public double Erhalten { get; set; }
        public SteuergruppeModel Steuer { get; set; }
    }
}
