using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infrastructure
{
    public static class GlobalVariables
    {
        private static RepositoryBase dbContext = null;

        public static RepositoryBase GetRepoBase()
        {
            dbContext = dbContext ?? new RepositoryBase();
            return dbContext;
        }

        public static void CreateRepoBase()
        {
            dbContext = dbContext ?? new RepositoryBase();
        }
    }
}

