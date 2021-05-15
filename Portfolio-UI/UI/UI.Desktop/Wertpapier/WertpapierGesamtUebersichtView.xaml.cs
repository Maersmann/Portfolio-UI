using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.DividendeViewModels;
using Aktien.Logic.UI.WertpapierViewModels;
using Aktien.UI.Desktop.Dividende;
using GalaSoft.MvvmLight.Messaging;
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

namespace Aktien.UI.Desktop.Wertpapier
{
    /// <summary>
    /// Interaktionslogik für WertpapierGesamtView.xaml
    /// </summary>
    public partial class WertpapierGesamtUebersichtView : UserControl
    {
        public WertpapierGesamtUebersichtView()
        {
            InitializeComponent();
        }

        public string MessageToken
        {
            set
            {
                if (this.DataContext is WertpapierGesamtUebersichtViewModel modelUebersicht)
                {
                    Messenger.Default.Register<OpenDividendenUebersichtAuswahlMessage>(this, value, m => ReceiveOpenDividendeUebersichtMessage(m));
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
