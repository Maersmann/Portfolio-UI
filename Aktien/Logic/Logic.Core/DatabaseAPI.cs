using Aktien.Data.Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core
{
    public class DatabaseAPI
    {
        public void AktualisereDatenbank()
        {
            Database dbAPI = new Database();
            dbAPI.OpenConnection();
        }
    }
}
