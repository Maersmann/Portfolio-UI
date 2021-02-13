﻿using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.WertpapierEntitys;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Aktien.Data.Infrastructure.AktienRepositorys
{
    public class DividendeErhaltenRepository: BaseRepository
    {
        public void Speichern(int? inID, DateTime inDatum, Double? inQuellensteuer, Double? inUmrechnungskurs, Double inGesamtBrutto, Double inGesamtNetto, Double inBestand, int inDividendeID, int inWertpapierID)
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
            Entity.WertpapierID = inWertpapierID;

            if (!inID.HasValue)
                repo.ErhaltendeDividenden.Add(Entity);

            repo.SaveChanges();
        }

        public void Speichern(DividendeErhalten inDividendeErhalten)
        {
            if (inDividendeErhalten.ID == 0)
                repo.ErhaltendeDividenden.Add(inDividendeErhalten);
            repo.SaveChanges();
        }

        public ObservableCollection<DividendeErhalten> LadeAllByWertpapierID(int inWertpapierID)
        {
            return new ObservableCollection<DividendeErhalten>(repo.ErhaltendeDividenden.Where(d => d.WertpapierID == inWertpapierID).OrderByDescending(o => o.Datum).ToList());
        }

        public DividendeErhalten LadeByID(int inID)
        {
            return repo.ErhaltendeDividenden.Where(d => d.ID == inID).First();
        }

        public DividendeErhalten LadeByDividendeID(int inID)
        {
            return repo.ErhaltendeDividenden.Where(d => d.DividendeID == inID).First();
        }

        public bool IstDividendeErhalten(int inDividendeID)
        {
            return repo.ErhaltendeDividenden.Where(a => a.DividendeID == inDividendeID).FirstOrDefault() != null;
        }

        public void Entfernen(int inID)
        {
            repo.ErhaltendeDividenden.Remove(repo.ErhaltendeDividenden.Find(inID));
            repo.SaveChanges();
        }
    }
}
