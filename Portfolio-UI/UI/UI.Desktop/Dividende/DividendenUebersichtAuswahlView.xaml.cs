using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.DividendeViewModels;
using Base.Logic.Messages;
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
            WeakReferenceMessenger.Default.Register<OpenDividendeUebersichtMessage>(this, (r,m)=> ReceiveOpenDividendeUebersichtMessage(m)); 
            WeakReferenceMessenger.Default.Register<OpenDividendeErhaltenUebersichtViewMessage>(this, (r, m) => ReceiveOpenDividendeErhaltenViewMessage(m));
        }

        private void ReceiveOpenDividendeErhaltenViewMessage(OpenDividendeErhaltenUebersichtViewMessage m)
        {
            Close();
            DividendeErhaltenUebersichtView view = new();

            if (view.DataContext is DividendeErhaltenUebersichtViewModel model)
                model.LoadData(m.WertpapierID);

            Window window = new()
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
            DividendenUebersichtView view = new();

             WeakReferenceMessenger.Default.Send(new AktualisiereViewMessage {  ID = m.WertpapierID }, StammdatenTypes.dividende.ToString());

            Window window = new()
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
            WeakReferenceMessenger.Default.Unregister<OpenDividendeUebersichtMessage>(this);
            WeakReferenceMessenger.Default.Unregister<OpenDividendeErhaltenUebersichtViewMessage>(this);
        }
    }
}
