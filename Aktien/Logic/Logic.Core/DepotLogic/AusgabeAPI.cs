using Aktien.Data.Infrastructure.DepotRepositorys;
using Aktien.Data.Model.DepotEntitys;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.DepotLogic
{
    public class AusgabeAPI
    {
        public ObservableCollection<Ausgabe> LadeAlle()
        {
            return new AusgabenRepository().LoadAll();
        }
    }
}
