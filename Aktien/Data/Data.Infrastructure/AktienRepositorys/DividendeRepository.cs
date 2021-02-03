using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.AktienModels;
using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Aktien.Data.Infrastructure.AktienRepositorys
{
    public class DividendeRepository : BaseRepository
    {
        public void Speichern(int? inID, Double inBetrag, DateTime inDatum, int? inAktieID, Waehrungen inWaehrung, Double? inBetragUmgerechnet )
        {
            var dividende = new Dividende();

            if (inID.HasValue)
                dividende = repo.Dividenden.Find(inID);
            else
                repo.Dividenden.Add(dividende);

            dividende.Betrag = inBetrag;
            dividende.Datum = inDatum;
            dividende.Waehrung = inWaehrung;
            dividende.BetragUmgerechnet = inBetragUmgerechnet;
            if (inAktieID.HasValue)
                dividende.AktieID = inAktieID.Value;
            repo.SaveChanges();
        }

        public ObservableCollection<Dividende> LadeAlleFuerAktie( int inAktieID )
        {
            return new ObservableCollection<Dividende>(repo.Dividenden.Where(d=>d.AktieID == inAktieID).OrderByDescending( d=>d.Datum ).ToList());
        }

        public Dividende LadeAnhandID(int inID)
        {
            return repo.Dividenden.Where(a => a.ID == inID).First();
        }

        public void Entfernen(int inID)
        {
            repo.Dividenden.Remove( repo.Dividenden.Find( inID ) );
            repo.SaveChanges();
        }
    }
}
