using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Messages.UtilMessages
{
    public class OpenBestaetigungViewMessage
    {
        public Action Command { get; set; }
        public string Beschreibung { get; set; }
    }
}
