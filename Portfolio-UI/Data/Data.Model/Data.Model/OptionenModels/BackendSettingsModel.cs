using Data.Types.OptionTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.OptionenModels
{
    public class BackendSettingsModel
    {
        public string Backend_IP { get; set; }
        public BackendProtokollTypes ProtokollTyp { get; set; }
        public int? Port { get; set; }
    }
}
