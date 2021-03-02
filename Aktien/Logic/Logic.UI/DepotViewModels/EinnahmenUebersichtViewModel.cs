using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class EinnahmenUebersichtViewModel : ViewModelUebersicht<Einnahme>
    {
        public EinnahmenUebersichtViewModel()
        {
            Title = "Übersicht aller Einnahmen";
            LoadData();
            RegisterAktualisereViewMessage(ViewType.viewEinnahmenUebersicht);
        }

        protected override int GetID() { return selectedItem.ID; }
        protected override ViewType GetStammdatenViewType() { return ViewType.viewEinnahmenStammdaten; }

        public override void LoadData()
        {
            var api = new EinnahmenAPI();
            itemList = api.LadeAlle();
            this.RaisePropertyChanged("ItemList");
        }



    }
}
