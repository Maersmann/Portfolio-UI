using Aktien.Logic.Messages.DepotMessages;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.UI.DepotViewModels;
using Aktien.Logic.UI.DividendeViewModels;
using Aktien.Logic.UI.WertpapierViewModels;
using Aktien.UI.Desktop.Dividende;
using Aktien.UI.Desktop.Wertpapier;
using GalaSoft.MvvmLight.Messaging;
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
            Messenger.Default.Register<OpenDividendenUebersichtAuswahlMessage>(this, "DepotUebersicht", m => ReceiveOpenDividendeUebersichtMessage(m));
            Messenger.Default.Register<OpenReverseSplitEintragenMessage>(this, "DepotUebersicht", m => ReceivOpenReverseSplitEintragenMessage(m));
            Messenger.Default.Register<OpenSplitEintragenMessage>(this, "DepotUebersicht", m => ReceiveOpenSplitEintragenMessage(m));
            Messenger.Default.Register<OpenErhalteneDividendeEintragenMessage>(this, "DepotUebersicht", m => ReceiveOpenDividendeErhaltenMessage(m));         
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

        private void ReceiveOpenDividendeUebersichtMessage(OpenDividendenUebersichtAuswahlMessage m)
        {
            DividendenUebersichtAuswahlView view = new DividendenUebersichtAuswahlView()
            {
                Owner = Application.Current.MainWindow
            };

            if (view.DataContext is DividendenUebersichtAuswahlViewModel model)
            {
                model.WertpapierID = m.WertpapierID;
            }

            _ = view.ShowDialog();
        }

        private void ReceiveOpenDividendeErhaltenMessage(OpenErhalteneDividendeEintragenMessage m)
        {
            ErhalteneDividendeEintragenView view = new ErhalteneDividendeEintragenView()
            {
                Owner = Application.Current.MainWindow
            };

            if (view.DataContext is ErhalteneDividendeEintragenViewModel model)
            {
                model.Wertpapier( m.WertpapierID, m.WertpapierName);
            }

            _ = view.ShowDialog();
        }

        private void ReceivOpenReverseSplitEintragenMessage(OpenReverseSplitEintragenMessage m)
        {
            ReverseSplitEintragenView view = new ReverseSplitEintragenView();
            if (view.DataContext is ReverseSplitEintragenViewModel model)
            {
                model.DepotWertpapierID = m.DepotWertpapierID;
            }

            Window window = new Window
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

        private void ReceiveOpenSplitEintragenMessage(OpenSplitEintragenMessage m)
        {
            SplitEintragenView view = new SplitEintragenView
            {
                Owner = Application.Current.MainWindow
            };

            if (view.DataContext is SplitEintragenViewModel model)
                model.DepotWertpapierID = m.DepotWertpapierID;

            _ = view.ShowDialog();
        }

    }
}