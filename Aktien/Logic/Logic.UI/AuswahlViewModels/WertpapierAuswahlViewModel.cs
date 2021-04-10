using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
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
        private WertpapierTypes WertpapierTypes;  
        public WertpapierAuswahlViewModel()
        {
            Title = "Auswahl Wertpapier";
            WertpapierTypes = WertpapierTypes.none;
            LoadData();
        }

        public void SetCallback(Action<bool, int> callback)
        {
            Callback = callback;
        }
        public void SetTyp(WertpapierTypes wertpapierTypes)
        {
            WertpapierTypes = wertpapierTypes;
            this.RaisePropertyChanged("CanAddNewItem");
            LoadData();
        }

        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.aktien; }

        public override void LoadData()
        {
            switch (WertpapierTypes)
            {
                case WertpapierTypes.none:
                    itemList = new WertpapierAPI().LadeAlle();
                    break;
                case WertpapierTypes.Aktie:
                    RegisterAktualisereViewMessage(StammdatenTypes.aktien);
                    itemList = new AktieAPI().LadeAlle();
                    break;
                case WertpapierTypes.ETF:
                    itemList = new EtfAPI().LadeAlle();
                    RegisterAktualisereViewMessage(StammdatenTypes.etf);
                    break;
                case WertpapierTypes.Derivate:
                    itemList = new DerivateAPI().LadeAlle();
                    RegisterAktualisereViewMessage(StammdatenTypes.derivate);
                    break;
                default:
                    itemList = new WertpapierAPI().LadeAlle();
                    break;
            }
            
            base.LoadData();
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
