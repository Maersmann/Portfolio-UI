using Data;
using Data.API;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
using Logic.Messages.Base;
using Logic.UI.BaseModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.UI.AktieUI
{
    public class AktienUebersichtViewModel : ViewModelBasis
    {
        private ObservableCollection<Aktie> alleAktien;
        public AktienUebersichtViewModel()
        {
            Title = "Alle Aktien";
            AktieAPI api = new AktieAPI();
            alleAktien = api.LadeAlle();
            Messenger.Default.Register<AktualisiereViewMessage>(this, m => ReceiveAktualisiereViewMessage());
        }

        private void ReceiveAktualisiereViewMessage()
        {
            AktieAPI api = new AktieAPI();
            alleAktien = api.LadeAlle();
            this.RaisePropertyChanged("AlleAktien");
        }

        public Aktie SelectedAktie { get; set; }

        public IEnumerable<Aktie> AlleAktien 
        {
            get
            {
                return alleAktien;
            }
        }
    }
}
