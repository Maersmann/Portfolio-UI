using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types.DividendenTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Aktien.Data.Infrastructure.AktienRepositorys
{
    public class DividendeErhaltenRepository: BaseRepository
    {
        public void Speichern(int? iD, Double? quellensteuer, Double? umrechnungskurs, Double gesamtBrutto, Double gesamtNetto, Double bestand, int dividendeID, int wertpapierID, DividendenRundungTypes typ, Double? gesamtNettoUmgerechnetErhalten, Double? gesamtNettoUmgerechnetErmittelt)
        {
            var Entity = new DividendeErhalten();
                
            if (iD.HasValue)
                Entity =repo.ErhaltendeDividenden.Find(iD.Value);

            Entity.Quellensteuer = quellensteuer;
            Entity.Umrechnungskurs = umrechnungskurs;
            Entity.GesamtBrutto = gesamtBrutto;
            Entity.GesamtNetto = gesamtNetto;
            Entity.Bestand = bestand;
            Entity.DividendeID = dividendeID;
            Entity.WertpapierID = wertpapierID;
            Entity.RundungArt = typ;
            Entity.GesamtNettoUmgerechnetErhalten = gesamtNettoUmgerechnetErhalten;
            Entity.GesamtNettoUmgerechnetErmittelt = gesamtNettoUmgerechnetErmittelt;

            if (!iD.HasValue)
                repo.ErhaltendeDividenden.Add(Entity);

            repo.SaveChanges();
        }

        public void Speichern(DividendeErhalten dividendeErhalten)
        {
            if (dividendeErhalten.ID == 0)
                repo.ErhaltendeDividenden.Add(dividendeErhalten);
            repo.SaveChanges();
        }

        public ObservableCollection<DividendeErhalten> LadeAlle()
        {
            return new ObservableCollection<DividendeErhalten>(repo.ErhaltendeDividenden.OrderBy(o => o.ID).ToList());
        }

        public ObservableCollection<DividendeErhalten> LadeAllByWertpapierID(int wertpapierID)
        {
            return new ObservableCollection<DividendeErhalten>(repo.ErhaltendeDividenden.Include(d => d.Dividende).Where(d => d.WertpapierID == wertpapierID).OrderByDescending(o => o.Dividende.Zahldatum).ToList());
        }

        public DividendeErhalten LadeByID(int iD)
        {
            return repo.ErhaltendeDividenden.Include(d => d.Dividende).Where(d => d.ID == iD).First();
        }

        public DividendeErhalten LadeByDividendeID(int iD)
        {
            return repo.ErhaltendeDividenden.Where(d => d.DividendeID == iD).First();
        }

        public bool IstDividendeErhalten(int dividendeID)
        {
            return repo.ErhaltendeDividenden.Where(a => a.DividendeID == dividendeID).FirstOrDefault() != null;
        }

        public void Entfernen(int iD)
        {
            repo.ErhaltendeDividenden.Remove(repo.ErhaltendeDividenden.Find(iD));
            repo.SaveChanges();
        }
    }
}
