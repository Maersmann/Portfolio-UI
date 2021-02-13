using Aktien.Data.Model.DepotEntitys;
using Aktien.Data.Types;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.Messages.EinnahmenMessages;
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
    public class EinnahmenUebersichtViewModel : ViewModelUebersicht
    {
        private ObservableCollection<Einnahme> einnahmen;

        private Einnahme selectedEinnahme;

        public EinnahmenUebersichtViewModel()
        {
            LoadData();
            AddAktieCommand = new RelayCommand(this.ExecuteAddAktieCommand);
            RegisterAktualisereViewMessage(ViewType.viewEinnahmenUebersicht);
        }


        public string MessageToken { set { messageToken = value; } }

        public override void LoadData()
        {
            var api = new EinnahmenAPI();
            einnahmen = api.LadeAlle();
            this.RaisePropertyChanged("Einnahmen");
        }


        #region Bindings
        public Einnahme SelectedEinnahme
        {
            get
            {
                return selectedEinnahme;
            }
            set
            {
                selectedEinnahme = value;
                this.RaisePropertyChanged();
            }
        }

        public IEnumerable<Einnahme> Einnahmen
        {
            get
            {
                return einnahmen;
            }
        }

        public ICommand AddAktieCommand { get; set; }
        #endregion

        #region Commands
        private void ExecuteAddAktieCommand()
        {
            Messenger.Default.Send<OpenEinnahmeStammdatenMessage>(new OpenEinnahmeStammdatenMessage { State = Data.Types.State.Neu });
        }
        #endregion
    }
}
