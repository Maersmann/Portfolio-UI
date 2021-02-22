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
        public void Speichern(int? iD, Double betrag, DateTime datum, int? wertpapierID, Waehrungen waehrung, Double? betragUmgerechnet, DividendenRundungTypes rundungTypes )
        {
            var dividende = new Dividende();

            if (iD.HasValue)
                dividende = repo.Dividenden.Find(iD);
            else
                repo.Dividenden.Add(dividende);

            dividende.Betrag = betrag;
            dividende.Datum = datum;
            dividende.Waehrung = waehrung;
            dividende.RundungArt = rundungTypes;
            if (betragUmgerechnet.HasValue)
                dividende.BetragUmgerechnet = Math.Round(betragUmgerechnet.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);

            if (wertpapierID.HasValue)
                dividende.WertpapierID = wertpapierID.Value;
            repo.SaveChanges();
        }

        public ObservableCollection<Dividende> LadeAlleFuerAktie( int wertpapierID )
        {
            return new ObservableCollection<Dividende>(repo.Dividenden.Where(d=>d.WertpapierID == wertpapierID).OrderByDescending( d=>d.Datum ).ToList());
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

        public Dividende LadeAnhandID(int iD)
        {
            return repo.Dividenden.Where(a => a.ID == iD).First();
        }

        public void Entfernen(int iD)
        {
            repo.Dividenden.Remove( repo.Dividenden.Find( iD ) );
            repo.SaveChanges();
        }

        public ObservableCollection<Dividende> LadeAlleNichtErhaltendeFuerWertpapier(int wertpapierID)
        {
            return new ObservableCollection<Dividende>(repo.Dividenden.Where(d => (d.WertpapierID == wertpapierID)).Where( d=> (!repo.ErhaltendeDividenden.Select(e => e.DividendeID).Contains(d.ID))).OrderByDescending(d => d.Datum).ToList());
        }

    }
}
