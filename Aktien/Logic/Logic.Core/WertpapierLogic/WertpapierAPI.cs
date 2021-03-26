using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Logic.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.WertpapierLogic
{
    public class WertpapierAPI : IAPI<Wertpapier>
    {
        public void Aktualisieren(Wertpapier entity)
        {
            throw new NotImplementedException();
        }

        public void Entfernen(int id)
        {
            throw new NotImplementedException();
        }

        public Wertpapier Lade(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Wertpapier> LadeAlle()
        {
            return new WertpapierRepository().LadeAlle();
        }

        public void Speichern(Wertpapier entity)
        {
            throw new NotImplementedException();
        }
    }
}
