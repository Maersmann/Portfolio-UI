using Aktien.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aktien.Data.Infrastructure.Base
{
    public class Database
    {
    
        public void OpenConnection()
        {
            GlobalVariables.CreateRepoBase();
        }
    }
}
