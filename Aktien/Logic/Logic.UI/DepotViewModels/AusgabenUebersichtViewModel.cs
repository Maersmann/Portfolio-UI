using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class AusgabenUebersichtViewModel : ViewModelUebersicht<Ausgabe>
    {
        public AusgabenUebersichtViewModel()
        {
            Title = "Übersicht aller Ausgaben";
            LoadData();
            RegisterAktualisereViewMessage(ViewType.viewAusgabenUebersicht);
        }

        protected override int GetID() { return selectedItem.ID; }
        protected override ViewType GetStammdatenViewType() { return ViewType.viewAusgabenStammdaten; }

        public override void LoadData()
        {
            var api = new AusgabeAPI();
            itemList = api.LadeAlle();
            this.RaisePropertyChanged("ItemList");
        }

    }
}
