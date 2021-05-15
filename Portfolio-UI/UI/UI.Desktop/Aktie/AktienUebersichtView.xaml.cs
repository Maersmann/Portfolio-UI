using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DepotMessages;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.AktieViewModels;
using Aktien.Logic.UI.DepotViewModels;
using Aktien.Logic.UI.DividendeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aktien.UI.Desktop.Aktie;
using Aktien.UI.Desktop.Depot;
using Aktien.UI.Desktop.Dividende;
using Aktien.Logic.Messages.AktieMessages;

namespace Aktien.UI.Desktop.Aktie
{
    /// <summary>
    /// Interaktionslogik für AktienUebersichtView.xaml
    /// </summary>
    public partial class AktienUebersichtView : UserControl
    {
        public AktienUebersichtView()
        {
            InitializeComponent();  
        }

        public string MessageToken
        {
            set
            {
                if (this.DataContext is AktienUebersichtViewModel modelUebersicht)
                {
                    Messenger.Default.Register<OpenDividendenUebersichtAuswahlMessage>(this, value , m => ReceiveOpenDividendeUebersichtMessage(m));
                    modelUebersicht.MessageToken = value;
                }
            }
        }

        private void ReceiveOpenDividendeUebersichtMessage(OpenDividendenUebersichtAuswahlMessage m)
        {
            var view = new DividendenUebersichtAuswahlView();

            if (view.DataContext is DividendenUebersichtAuswahlViewModel model)
                model.WertpapierID = m.WertpapierID;
            view.ShowDialog();
        }
    }
}
