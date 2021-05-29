using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Messages.SteuernMessages
{
    public class OpenSteuernUebersichtMessage
    {
        public int? SteuergruppeID { get; set; }

        public Action<bool, int> Callback { get; private set; }
        public bool IstVerknuepfungGespeichert { get; set; }

        public OpenSteuernUebersichtMessage(Action<bool, int> callback, int? steuergruppeID, bool istVerknuepfungGespeichert)
        {
            Callback = callback;
            SteuergruppeID = steuergruppeID;
            IstVerknuepfungGespeichert = istVerknuepfungGespeichert;
        }
    }
}
