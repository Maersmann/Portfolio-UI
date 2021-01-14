using Aktien.Data.Model.DepotModels;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.UI.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class DepotUebersichtViewModel: ViewModelBasis
    {
        private readonly ObservableCollection<DepotAktie> depotAktien;

        private DepotAktie selectedDepotAktie;

        public DepotUebersichtViewModel()
        {
            var api = new DepotAPI();
            depotAktien = api.LadeAlleVorhandeneImDepot();
        }


        #region Bindings
        public DepotAktie SelectedDepotAktie
        {
            get
            {
                return selectedDepotAktie;
            }
            set
            {
                selectedDepotAktie = value;
                this.RaisePropertyChanged();
            }
        }

        public IEnumerable<DepotAktie> DepotAktien
        {
            get
            {
                return depotAktien;
            }
        }
        #endregion
    }
}
