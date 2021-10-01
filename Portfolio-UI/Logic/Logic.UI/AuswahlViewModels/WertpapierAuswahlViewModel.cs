using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.AuswahlModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Aktien.Logic.UI.AuswahlViewModels
{
    public class WertpapierAuswahlViewModel : ViewModelAuswahl<WertpapierAuswahlModel, StammdatenTypes>
    {
        
        private Action<bool, int> Callback;
        private WertpapierTypes WertpapierTypes;
        public WertpapierAuswahlViewModel()
        {
            Title = "Auswahl Wertpapier";
            
            WertpapierTypes = WertpapierTypes.none;
            RegisterAktualisereViewMessage(StammdatenTypes.aktien.ToString());
        }

        public void SetCallback(Action<bool, int> callback)
        {
            Callback = callback;
        }
        public void SetTyp(WertpapierTypes wertpapierTypes)
        {
            WertpapierTypes = wertpapierTypes;
            RaisePropertyChanged(nameof(CanAddNewItem));
            LoadData();
        }

        protected override string GetREST_API() => $"/api/Wertpapier?aktiv=true&type={WertpapierTypes}";
        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.aktien; }


        protected override bool OnFilterTriggered(object item)
        {
            if (item is WertpapierAuswahlModel wertpapier)
            {
                return wertpapier.Name.ToLower().Contains(filtertext.ToLower());
            }
            return true;
        }


        #region Bindings
        public bool CanAddNewItem => WertpapierTypes != WertpapierTypes.none;

        #endregion

        #region commands
        protected override void ExecuteCloseWindowCommand(Window window)
        {
            base.ExecuteCloseWindowCommand(window);

            if (selectedItem != null)
                Callback(true, selectedItem.ID);
            else
                Callback(false, 0);
        }
        
        #endregion
        

    }
}
