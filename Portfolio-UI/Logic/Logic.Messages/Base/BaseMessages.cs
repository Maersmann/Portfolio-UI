﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Messages.Base
{

    public class StammdatenGespeichertMessage
    {
        public bool Erfolgreich { get; set; }
        public string Message { get; set; }
    }
}
