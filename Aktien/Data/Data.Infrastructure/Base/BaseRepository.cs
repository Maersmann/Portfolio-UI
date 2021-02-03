using Aktien.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aktien.Data.Infrastructure.Base
{
    public class BaseRepository
    {
        protected readonly Repository repo;
        public BaseRepository()
        {
            repo = GlobalVariables.GetRepoBase();
        }
    }
}
