using Data.Model.SparplanModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Messages.SparplanMessages
{
    public class OpenSparplanAusfuehrenMessage
    {
        public SparplanAusfuehrenUebersichtModel SparplanAusfuehren { get; set; }
    }
}
