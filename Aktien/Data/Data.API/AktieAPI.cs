using Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.API
{
    public class AktieAPI
    {
        private readonly RepositoryBase repo;
        public AktieAPI()
        {
            repo = GlobalVariables.GetRepoBase();
        }

        public void Speichern(Aktie aktie)
        {
            repo.Aktien.Add(aktie);
            repo.SaveChanges();
        }
    }
}
