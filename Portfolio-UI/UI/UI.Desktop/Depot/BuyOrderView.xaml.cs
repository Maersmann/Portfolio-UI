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
using UI.Desktop.Base;
using Data.Types.SteuerTypes;
using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.SteuernMessages;
using Logic.UI.SteuerViewModels;
using UI.Desktop.Steuer;
using Aktien.Data.Types;

namespace UI.Desktop.Depot
{
    /// <summary>
    /// Interaktionslogik für AktieGekauftView.xaml
    /// </summary>
    public partial class BuyOrderView : StammdatenView
    {
        public BuyOrderView()
        {
            InitializeComponent();
            base.RegisterStammdatenGespeichertMessage(StammdatenTypes.buysell);
            WeakReferenceMessenger.Default.Register<OpenSteuernUebersichtMessage, string>(this, "BuyOrder", (r,m) => ReceiveOpenSteuernUebersichtMessage(m));
        }

        private void ReceiveOpenSteuernUebersichtMessage(OpenSteuernUebersichtMessage m)
        {
            var view = new SteuernUebersichtView();
            if (view.DataContext is SteuernUebersichtViewModel model)
            {
                model.SetCallback(m.Callback);
                model.setSteuern(m.Steuern);
            }
            view.Owner = Application.Current.MainWindow;
            view.ShowDialog();

        }

        public override void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            base.Window_Unloaded(sender, e);
            WeakReferenceMessenger.Default.Unregister<OpenSteuernUebersichtMessage, string>(this, "BuyOrder");
        }
    }
}
