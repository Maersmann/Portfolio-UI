using Aktien.Data.Model.WertpapierModels;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.WertpapierViewModels
{
    public class WertpapierGesamtUebersichtViewModel : ViewModelBasis
    {
        private ObservableCollection<Wertpapier> wertpapiere;

        private Wertpapier selectedWertpapier;

        public WertpapierGesamtUebersichtViewModel()
        {
            LoadData();
            messageToken = "";
        }

        public string MessageToken { set { messageToken = value; } }
        public void LoadData()
        {
            wertpapiere = new WertpapierAPI().LadeAlle();
            this.RaisePropertyChanged("Wertpapiere");
        }


        #region Bindings
        public Wertpapier SelectedWertpapier
        {
            get
            {
                return selectedWertpapier;
            }
            set
            {
                selectedWertpapier = value;
                this.RaisePropertyChanged();
                if (selectedWertpapier != null)
                {
                    Messenger.Default.Send<LoadWertpapierOrderMessage>(new LoadWertpapierOrderMessage { WertpapierID = selectedWertpapier.ID }, messageToken);
                }
            }
        }
        public IEnumerable<Wertpapier> Wertpapiere
        {
            get
            {
                return wertpapiere;
            }
        }
        #endregion
    }
}
