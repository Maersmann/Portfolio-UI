using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Messages.Aktie
{
    public class SaveNeueAktieResultMessage
    {
        public bool Erfolgreich { get; set; }

        public string Message { get; set; }
    }

    public class OpenAktieStammdatenBearbeitenMessage
    {
        public int AktieID { get; set; }
    }

    public class DeleteAktieErfolgreichMessage
    {

    }
}
