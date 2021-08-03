using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.OptionenModels
{
    public class InfoModel
    {
        public string VersionBackend { get; set; }
        public string VersionFrontend { get; set; }
        public DateTime ReleaseBackend { get; set; }
        public DateTime ReleaseFronted { get; set; }
    }
}
