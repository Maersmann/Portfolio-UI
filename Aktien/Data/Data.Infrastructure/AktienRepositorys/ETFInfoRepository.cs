using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types.WertpapierTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Aktien.Data.Infrastructure.AktienRepositorys
{
    public class ETFInfoRepository : BaseRepository
    {
        public void Speichern(int? iD, ErmittentTypes emittend, ProfitTypes profitTyp, int wertpapierID)
        {
            var Entity = new ETFInfo();
            if (iD.GetValueOrDefault(0) == 0) iD = null;
            if (iD.HasValue)
                Entity = repo.ETFInfos.Find(iD.Value);

            Entity.Emittent = emittend;
            Entity.ProfitTyp = profitTyp;
            Entity.WertpapierID = wertpapierID;

            if (!iD.HasValue)
                repo.ETFInfos.Add(Entity);

            repo.SaveChanges();
        }

        public ObservableCollection<ETFInfo> LadeAlle()
        {
            return new ObservableCollection<ETFInfo>(repo.ETFInfos.OrderBy(o => o.ID).ToList());
        }

        public ETFInfo LadeAnhandID(int iD)
        {
            return repo.ETFInfos.Where(a => a.ID == iD).First();
        }
    }
}
