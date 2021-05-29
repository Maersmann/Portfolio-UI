using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.DividendeModels
{
    public class DividendeErhaltenStammdatenModel : DividendeErhaltenModel
    {
        public double? Zwischensumme { get; set; }
        public double? ErhaltenUmgerechnetErmittelt { get; set; }
        public double? SteuernVorZwischensumme { get; set; }
        public double? SteuernNachZwischensumme { get; set; }
        public double? ZwischensummeUmgerechnet { get; set; }
    }
}
