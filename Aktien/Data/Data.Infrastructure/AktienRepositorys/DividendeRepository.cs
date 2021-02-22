using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Aktien.Data.Infrastructure.AktienRepositorys
{
    public class DividendeRepository : BaseRepository
    {
        public void Speichern(int? inID, Double inBetrag, DateTime inDatum, int? inWertpapierID, Waehrungen inWaehrung, Double? inBetragUmgerechnet, DividendenRundungTypes rundungTypes )
        {
            var dividende = new Dividende();

            if (inID.HasValue)
                dividende = repo.Dividenden.Find(inID);
            else
                repo.Dividenden.Add(dividende);

            dividende.Betrag = inBetrag;
            dividende.Datum = inDatum;
            dividende.Waehrung = inWaehrung;
            dividende.RundungArt = rundungTypes;
            if (inBetragUmgerechnet.HasValue)
                dividende.BetragUmgerechnet = Math.Round(inBetragUmgerechnet.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);

            if (inWertpapierID.HasValue)
                dividende.WertpapierID = inWertpapierID.Value;
            repo.SaveChanges();
        }

        public ObservableCollection<Dividende> LadeAlleFuerAktie( int inWertpapierID )
        {
            return new ObservableCollection<Dividende>(repo.Dividenden.Where(d=>d.WertpapierID == inWertpapierID).OrderByDescending( d=>d.Datum ).ToList());
        }

        public void Speichern(Dividende dividende)
        {
            if (dividende.ID == 0)
                repo.Dividenden.Add(dividende);

            repo.SaveChanges();
        }

        public ObservableCollection<Dividende> LadeAlle()
        {
            return new ObservableCollection<Dividende>(repo.Dividenden.OrderBy(o => o.ID).ToList());
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

        public ObservableCollection<Dividende> LadeAlleNichtErhaltendeFuerWertpapier(int inWertpapierID)
        {
            return new ObservableCollection<Dividende>(repo.Dividenden.Where(d => (d.WertpapierID == inWertpapierID)).Where( d=> (!repo.ErhaltendeDividenden.Select(e => e.DividendeID).Contains(d.ID))).OrderByDescending(d => d.Datum).ToList());
        }

    }
}
