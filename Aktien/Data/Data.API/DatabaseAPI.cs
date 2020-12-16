using Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.API
{
    public class DatabaseAPI
    {
        
        public void OpenConnection()
        {
            GlobalVariables.CreateRepoBase();
        }
    }
}
