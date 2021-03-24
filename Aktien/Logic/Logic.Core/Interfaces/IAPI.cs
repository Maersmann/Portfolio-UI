using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.Core.Interfaces
{
    public interface IAPI<T>
    {
        void Speichern(T entity);
        void Aktualisieren(T entity);
        ObservableCollection<T> LadeAlle();
        void Entfernen(int id);
        T Lade(int id);

    }
}
