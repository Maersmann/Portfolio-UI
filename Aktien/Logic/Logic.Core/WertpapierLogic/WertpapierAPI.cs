using Aktien.Data.Infrastructure.AktienRepositorys;
using Aktien.Data.Model.WertpapierEntitys;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.WertpapierLogic
{
    public class WertpapierAPI
    {
        public ObservableCollection<Wertpapier> LadeAlle()
        {
            return new WertpapierRepository().LadeAlle();
        }
    }
}
