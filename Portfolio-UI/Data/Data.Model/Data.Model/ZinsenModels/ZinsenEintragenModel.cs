using Data.Model.SteuerModels;
using Data.Types.SteuerTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.ZinsenModels
{
    public class ZinsenEintragenModel
    {
        public int ID { get; set; }
        public DateTime? ErhaltenAm { get; set; }
        public String Gesamt { get; set; }
        public String Prozent { get; set; }
        public String DurchschnittlicherKontostand { get; set; }
        public String Erhalten { get; set; }
        public String SteuernGesamt { get; set; }

        public SteuergruppeModel Steuer { get; set; }
        public string Monat { get; set; }
        public string Jahr { get; set; }

        public ZinsenEintragenModel() 
        {
            Steuer = new SteuergruppeModel { SteuerHerkunftTyp = SteuerHerkunftTyp.shtZinsen,  Steuern = new List<SteuerModel>() };
        }
    }
}
