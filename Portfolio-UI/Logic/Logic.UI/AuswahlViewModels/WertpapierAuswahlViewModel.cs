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
        
        private WertpapierTypes WertpapierTypes;
        public WertpapierAuswahlViewModel()
        {
            Title = "Auswahl Wertpapier";          
            WertpapierTypes = WertpapierTypes.none;
            RegisterAktualisereViewMessage(StammdatenTypes.aktien.ToString());
        }
        protected override bool LoadingOnCreate() => false;
        protected override bool WithPagination() { return true; }

        public async void SetTyp(WertpapierTypes wertpapierTypes)
        {
            WertpapierTypes = wertpapierTypes;
            RaisePropertyChanged(nameof(CanAddNewItem));
            await LoadData();
        }

        public int? ID()
        {
            return SelectedItem == null ? null : (int?)SelectedItem.ID;
        }

        protected override string GetREST_API() => $"/api/Wertpapier?aktiv=true&type={WertpapierTypes}";
        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.aktien; }


        #region Bindings
        public bool CanAddNewItem => WertpapierTypes != WertpapierTypes.none;

        #endregion

        #region commands
        protected override void ExecuteCloseWindowCommand(Window window)
        {
            AuswahlGetaetigt = true;
            base.ExecuteCloseWindowCommand(window);
        }

        #endregion


    }
}
