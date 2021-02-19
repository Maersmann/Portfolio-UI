using Aktien.Data.Infrastructure.Base;
using Aktien.Data.Infrastructure.OptionRepositorys;
using Aktien.Data.Types.OptionTypes;
using Aktien.Logic.Core.KonvertierungLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core
{
    public class DatabaseAPI
    {
        public void AktualisereDatenbank()
        {
            Database dbAPI = new Database();
            dbAPI.OpenConnection();

            var konv = new KonvertierungRepository();

            if (!konv.IstVorhanden(KonvertierungTypes.konvertierungEinAusgabenUndRunden))
            {
                new KonvertierungRunden().Start();
                new KonvertierungEinnahmenAusgaben().Start();
                konv.Speichern(KonvertierungTypes.konvertierungEinAusgabenUndRunden);
            }
            

        }
    }
}
