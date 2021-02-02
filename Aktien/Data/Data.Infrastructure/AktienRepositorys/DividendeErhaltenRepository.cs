using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.AktienModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aktien.Data.Infrastructure.AktienRepositorys
{
    public class DividendeErhaltenRepository: BaseRepository
    {
        public void Speichern(DividendeErhalten inDividendeErhalten )
        {
            repo.ErhaltendeDividenden.Add(inDividendeErhalten);
            repo.SaveChanges();
        }
    }
}
