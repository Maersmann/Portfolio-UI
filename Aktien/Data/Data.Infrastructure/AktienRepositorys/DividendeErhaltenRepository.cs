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
        public void Speichern(int? inID, DateTime inDatum, Double? inQuellensteuer, Double? inUmrechnungskurs, Double inGesamtBrutto, Double inGesamtNetto, Double inBestand, int inDividendeID, int inAktieID)
        {
            var Entity = new DividendeErhalten();
                
            if (inID.HasValue)
                Entity =repo.ErhaltendeDividenden.Find(inID.Value);

            Entity.Datum = inDatum;
            Entity.Quellensteuer = inQuellensteuer;
            Entity.Umrechnungskurs = inUmrechnungskurs;
            Entity.GesamtBrutto = inGesamtBrutto;
            Entity.GesamtNetto = inGesamtNetto;
            Entity.Bestand = inBestand;
            Entity.DividendeID = inDividendeID;
            Entity.AktieID = inAktieID;

            if (!inID.HasValue)
                repo.ErhaltendeDividenden.Add(Entity);

            repo.SaveChanges();
        }

        public ObservableCollection<DividendeErhalten> LadeAllByAktieID(int inAktieID)
        {
            return new ObservableCollection<DividendeErhalten>(repo.ErhaltendeDividenden.Where(d => d.AktieID == inAktieID).OrderByDescending(o => o.Datum).ToList());
        }

        public DividendeErhalten LadeByID(int inID)
        {
            return repo.ErhaltendeDividenden.Where(d => d.ID == inID).First();
        }

        public void Entfernen(int inID)
        {
            repo.ErhaltendeDividenden.Remove(repo.ErhaltendeDividenden.Find(inID));
            repo.SaveChanges();
        }
    }
}
