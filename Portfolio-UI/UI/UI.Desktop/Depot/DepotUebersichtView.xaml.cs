using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.DepotMessages;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.UI.DepotViewModels;
using Aktien.Logic.UI.DividendeViewModels;
using Aktien.Logic.UI.WertpapierViewModels;
using Aktien.UI.Desktop.Dividende;
using Aktien.UI.Desktop.Wertpapier;
using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.DepotMessages;
using Logic.Messages.WertpapierMessages;
using Logic.UI.DepotViewModels;
using Logic.UI.WertpapierViewModels;
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
using UI.Desktop.Depot;
using UI.Desktop.Wertpapier;

namespace Aktien.UI.Desktop.Depot
{
    /// <summary>
    /// Interaktionslogik für DepotUebersichtView.xaml
    /// </summary>
    public partial class DepotUebersichtView : UserControl
    {
        public DepotUebersichtView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<OpenDividendenUebersichtAuswahlMessage, string>(this, "DepotUebersicht", (r, m) => ReceiveOpenDividendeUebersichtMessage(m));
            WeakReferenceMessenger.Default.Register<OpenReverseSplitEintragenMessage, string>(this, "DepotUebersicht", (r, m) => ReceivOpenReverseSplitEintragenMessage(m));
            WeakReferenceMessenger.Default.Register<OpenSplitEintragenMessage, string>(this, "DepotUebersicht", (r, m) => ReceiveOpenSplitEintragenMessage(m));
            WeakReferenceMessenger.Default.Register<OpenErhalteneDividendeEintragenMessage, string>(this, "DepotUebersicht", (r,m) => ReceiveOpenDividendeErhaltenMessage(m));         
        }

        public string MessageToken
        {
            set
            {
               
                if (DataContext is DepotUebersichtViewModel modelUebersicht)
                {
                    modelUebersicht.MessageToken = value;
                }
            }
        }

        private static void ReceiveOpenDividendeUebersichtMessage(OpenDividendenUebersichtAuswahlMessage m)
        {
            DividendenUebersichtAuswahlView view = new()
            {
                Owner = Application.Current.MainWindow
            };

            if (view.DataContext is DividendenUebersichtAuswahlViewModel model)
            {
                model.WertpapierID = m.WertpapierID;
            }

            _ = view.ShowDialog();
        }

        private static void ReceiveOpenDividendeErhaltenMessage(OpenErhalteneDividendeEintragenMessage m)
        {
            ErhalteneDividendeEintragenView view = new()
            {
                Owner = Application.Current.MainWindow
            };

            if (view.DataContext is ErhalteneDividendeEintragenViewModel model)
            {
                model.Wertpapier( m.WertpapierID, m.WertpapierName);
            }

            _ = view.ShowDialog();
        }

        private static void ReceivOpenReverseSplitEintragenMessage(OpenReverseSplitEintragenMessage m)
        {
            ReverseSplitEintragenView view = new();
            if (view.DataContext is ReverseSplitEintragenViewModel model)
            {
                model.DepotWertpapierID = m.DepotWertpapierID;
            }

            Window window = new()
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow
            };

            _ = window.ShowDialog();
        }

        private static void ReceiveOpenSplitEintragenMessage(OpenSplitEintragenMessage m)
        {
            SplitEintragenView view = new()
            {
                Owner = Application.Current.MainWindow
            };

            if (view.DataContext is SplitEintragenViewModel model)
                model.DepotWertpapierID = m.DepotWertpapierID;

            _ = view.ShowDialog();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<OpenDividendenUebersichtAuswahlMessage, string>(this, "DepotUebersicht");
            WeakReferenceMessenger.Default.Unregister<OpenReverseSplitEintragenMessage, string>(this, "DepotUebersicht");
            WeakReferenceMessenger.Default.Unregister<OpenSplitEintragenMessage, string>(this, "DepotUebersicht");
            WeakReferenceMessenger.Default.Unregister<OpenErhalteneDividendeEintragenMessage, string>(this, "DepotUebersicht");

        }
    }
}