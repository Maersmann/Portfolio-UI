using Aktien.Logic.Messages.Base;
using Base.Logic.Messages;
using Data.Types.SteuerTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Messages.SteuernMessages
{
    public class OpenSteuerStammdatenMessage<T> : BaseStammdatenMessage<T>
    {
        public int? SteuergruppeID { get; set; }
        public SteuerHerkunftTyp Typ { get; set; }
        public bool IstVerknuepfungGespeichert { get; set; }
    }
}
