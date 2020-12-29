using Data.Entity.AktieEntitys;
using Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Data.API
{
    public class DividendeAPI
    {
        private readonly RepositoryBase repo;
        public DividendeAPI()
        {
            repo = GlobalVariables.GetRepoBase();
        }
        public void SpeicherNeueDividende( Double inBetrag, DateTime inDatum, int inAktieID )
        {
            repo.Dividenden.Add(new Dividende { AktieID = inAktieID, Betrag = inBetrag, Datum = inDatum });
            repo.SaveChanges();
        }

        public ObservableCollection<Dividende> LadeAlleFuerAktie( int inAktieID )
        {
            return new ObservableCollection<Dividende>(repo.Dividenden.Where(d=>d.AktieID == inAktieID).OrderBy( d=>d.Datum ).ToList());
        }
    }
}
