using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.AktienModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public ObservableCollection<DividendeErhalten> LadeAllByAktieID(int inAktieID)
        {
            return new ObservableCollection<DividendeErhalten>(repo.ErhaltendeDividenden.Where(d => d.AktieID == inAktieID).OrderByDescending(o => o.Datum).ToList());
        }
    }
}
