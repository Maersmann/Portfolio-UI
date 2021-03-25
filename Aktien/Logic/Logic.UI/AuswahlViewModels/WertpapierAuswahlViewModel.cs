using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.UI.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Aktien.Logic.UI.AuswahlViewModels
{
    public class WertpapierAuswahlViewModel : ViewModelAuswahl<Wertpapier>
    {
        private Action<bool, int> Callback;
        public WertpapierAuswahlViewModel()
        {
            Title = "Auswahl Wertpapier";
            LoadData();
        }

        public void SetCallback(Action<bool, int> callback)
        {
            Callback = callback;
        }

        public int? WertpapierID()
        {
            if (selectedItem == null) return null;
            else return selectedItem.ID;
        }

        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.aktien; }

        public override void LoadData()
        {
            itemList = new WertpapierAPI().LadeAlle();
            base.LoadData();
        }

        protected override void ExecuteCloseWindowCommand(Window window)
        {
            base.ExecuteCloseWindowCommand(window);

            if (selectedItem != null)
                Callback(true, selectedItem.ID);
            else
                Callback(false, 0);
        }
    }
}
