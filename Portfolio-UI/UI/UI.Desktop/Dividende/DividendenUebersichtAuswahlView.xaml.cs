using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.DividendeViewModels;
using Base.Logic.Messages;
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
using System.Windows.Shapes;
using UI.Desktop.Dividende;

namespace Aktien.UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendenUebersichtAuswahl.xaml
    /// </summary>
    public partial class DividendenUebersichtAuswahlView : Window
    {
        public DividendenUebersichtAuswahlView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenDividendeUebersichtMessage>(this, m => ReceiveOpenDividendeUebersichtMessage(m)); 
            Messenger.Default.Register<OpenDividendeErhaltenUebersichtViewMessage>(this, m => ReceiveOpenDividendeErhaltenViewMessage(m));
        }

        private void ReceiveOpenDividendeErhaltenViewMessage(OpenDividendeErhaltenUebersichtViewMessage m)
        {
            Close();
            DividendeErhaltenUebersichtView view = new DividendeErhaltenUebersichtView();

            if (view.DataContext is DividendeErhaltenUebersichtViewModel model)
                model.LoadData(m.WertpapierID);

            Window window = new Window
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow
            };

            window.ShowDialog();
        }

        private void ReceiveOpenDividendeUebersichtMessage(OpenDividendeUebersichtMessage m)
        {
            Close();
            DividendenUebersichtView view = new DividendenUebersichtView();

            Messenger.Default.Send(new AktualisiereViewMessage {  ID = m.WertpapierID }, StammdatenTypes.dividende.ToString());

            Window window = new Window
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow
            };

            window.ShowDialog();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<OpenDividendeUebersichtMessage>(this);
            Messenger.Default.Unregister<OpenDividendeErhaltenUebersichtViewMessage>(this);
        }
    }
}
