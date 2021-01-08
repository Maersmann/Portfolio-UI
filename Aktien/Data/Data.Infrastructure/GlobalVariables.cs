using System;
using System.Collections.Generic;
using System.Text;

namespace Aktien.Data.Infrastructure
{
    public static class GlobalVariables
    {
        private static Repository dbContext = null;

        public static Repository GetRepoBase()
        {
            dbContext = dbContext ?? new Repository();
            return dbContext;
        }

        public static void CreateRepoBase()
        {
            dbContext = dbContext ?? new Repository();
        }
    }
}

