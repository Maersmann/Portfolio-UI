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
        public void Speichern(int? inID, ErmittentTypes inEmittend, ProfitTypes inProfitTyp, int inWertpapierID)
        {
            var Entity = new ETFInfo();
            if (inID.GetValueOrDefault(0) == 0) inID = null;
            if (inID.HasValue)
                Entity = repo.ETFInfos.Find(inID.Value);

            Entity.Emittent = inEmittend;
            Entity.ProfitTyp = inProfitTyp;
            Entity.WertpapierID = inWertpapierID;

            if (!inID.HasValue)
                repo.ETFInfos.Add(Entity);

            repo.SaveChanges();
        }

        public ObservableCollection<ETFInfo> LadeAlle()
        {
            return new ObservableCollection<ETFInfo>(repo.ETFInfos.OrderBy(o => o.ID).ToList());
        }

        public ETFInfo LadeAnhandID(int inID)
        {
            return repo.ETFInfos.Where(a => a.ID == inID).First();
        }
    }
}
