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
using Aktien.UI.Desktop.Base;
using Data.Types.SteuerTypes;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.SteuernMessages;
using Logic.UI.SteuerViewModels;
using UI.Desktop.Steuer;

namespace Aktien.UI.Desktop.Depot
{
    /// <summary>
    /// Interaktionslogik für AktieGekauftView.xaml
    /// </summary>
    public partial class BuyOrderView : StammdatenView
    {
        public BuyOrderView()
        {
            InitializeComponent();
            base.RegisterStammdatenGespeichertMessage(Data.Types.StammdatenTypes.buysell);
            Messenger.Default.Register<OpenSteuernUebersichtMessage>(this, "BuyOrder", m => ReceiveOpenSteuernUebersichtMessage(m));
        }

        private void ReceiveOpenSteuernUebersichtMessage(OpenSteuernUebersichtMessage m)
        {
            var view = new SteuernUebersichtView();
            if (view.DataContext is SteuernUebersichtViewModel model)
            {
                model.SetCallback(m.Callback);
                model.setSteuern(m.Steuern);
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
            window.ShowDialog();

        }

        public override void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            base.Window_Unloaded(sender, e);
            Messenger.Default.Unregister<OpenSteuernUebersichtMessage>(this);
        }
    }
}
