using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DepotLogic;
using Base.Logic.ViewModels;
using Data.Model.DepotModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class EinnahmenUebersichtViewModel : ViewModelUebersicht<EinnahmeModel, StammdatenTypes>
    {
        public EinnahmenUebersichtViewModel()
        {
            Title = "Übersicht aller Einnahmen";
            RegisterAktualisereViewMessage(StammdatenTypes.einnahmen.ToString());
            RegisterAktualisereViewMessage(StammdatenTypes.dividendeErhalten.ToString());
            RegisterAktualisereViewMessage(StammdatenTypes.steuer.ToString());
            RegisterAktualisereViewMessage(StammdatenTypes.buysell.ToString());
        }

        protected override int GetID() { return SelectedItem.ID; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.einnahmen; }
        protected override string GetREST_API() { return $"/api/depot/Einnahmen"; }
        protected override bool WithPagination() { return true; }

    }
}
