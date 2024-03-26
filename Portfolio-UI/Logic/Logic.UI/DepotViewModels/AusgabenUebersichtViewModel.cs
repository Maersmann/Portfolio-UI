using Aktien.Data.Types;
using Data.Model.DepotModels;
using System.Collections.ObjectModel;
using System.Net.Http;
using Base.Logic.ViewModels;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class AusgabenUebersichtViewModel : ViewModelUebersicht<AusgabeModel, StammdatenTypes>
    {
        public AusgabenUebersichtViewModel()
        {
            Title = "Übersicht aller Ausgaben";
            RegisterAktualisereViewMessage(StammdatenTypes.ausgaben.ToString());
        }

        protected override int GetID() { return SelectedItem.ID; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.ausgaben; }
        protected override string GetREST_API() { return $"/api/depot/Ausgaben"; }
        protected override bool WithPagination() { return true; }
    }
}
