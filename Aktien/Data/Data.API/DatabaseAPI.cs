using System;
using System.Collections.Generic;
using System.Text;

namespace Data.API
{
    public class DatabaseAPI
    {
        private static RepositoryBase repoBase = null;

        public void OpenConnection()
        {
            repoBase = repoBase ?? new RepositoryBase();
        }
    }
}
