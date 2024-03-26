using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.DividendeViewModels;
using Aktien.Logic.UI.ETFViewModels;
using Aktien.UI.Desktop.Dividende;
using CommunityToolkit.Mvvm.Messaging;
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

namespace Aktien.UI.Desktop.ETF
{
    /// <summary>
    /// Interaktionslogik für ETFGesamtUebersicht.xaml
    /// </summary>
    public partial class ETFGesamtUebersichtView : UserControl
    {
        private string token;
        public ETFGesamtUebersichtView()
        {
            InitializeComponent();
        }

        public string MessageToken 
        { 
            set 
            {
                if (this.DataContext is ETFGesamtUebersichtViewModel modelUebersicht)
                {
                    token = value;
                    WeakReferenceMessenger.Default.Register<OpenDividendenUebersichtAuswahlMessage, string>(this, value, (r,m)=> ReceiveOpenDividendeUebersichtMessage(m));
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

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<OpenDividendenUebersichtAuswahlMessage, string>(this, token);
        }
    }
}
