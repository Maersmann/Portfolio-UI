using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.Messages.AusgabenMessages;
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
            LoadData();
            AddAktieCommand = new RelayCommand(this.ExecuteAddAktieCommand);
            RegisterAktualisereViewMessage(ViewType.viewAusgabenUebersicht);
        }


        public string MessageToken { set { messageToken = value; } }

        public override void LoadData()
        {
            var api = new AusgabeAPI();
            itemList = api.LadeAlle();
            this.RaisePropertyChanged("ItemList");
        }


        #region Bindings

        public ICommand AddAktieCommand { get; set; }
        #endregion

        #region Commands
        private void ExecuteAddAktieCommand()
        {
            Messenger.Default.Send<OpenAusgabeStammdatenMessage>(new OpenAusgabeStammdatenMessage { State = Data.Types.State.Neu });
        }
        #endregion
    }
}
