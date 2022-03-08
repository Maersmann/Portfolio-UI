using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.AuswahlViewModels;
using Aktien.Logic.UI.DividendeViewModels;
using Aktien.UI.Desktop.Auswahl;
using Aktien.UI.Desktop.Base;
using Data.Types.SteuerTypes;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.SteuernMessages;
using Logic.UI.SteuerViewModels;
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
using UI.Desktop.Steuer;

namespace Aktien.UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendeErhaltenView.xaml
    /// </summary>
    public partial class DividendeErhaltenView : StammdatenView
    {
        public DividendeErhaltenView()
        {
            InitializeComponent();
            RegisterStammdatenGespeichertMessage(Data.Types.StammdatenTypes.dividendeErhalten);
            Messenger.Default.Register<OpenDividendenAuswahlMessage>(this, "DividendeErhalten", m => ReceiveOpenDividendeAuswahlMessage(m));
            Messenger.Default.Register<OpenSteuernUebersichtMessage>(this, "DividendeErhalten", m => ReceiveOpenSteuernUebersichtMessage(m));
            Messenger.Default.Register<OpenDividendeProStueckAnpassenMessage>(this, m => ReceiveOpenDividendeProStueckAnpassenMessage(m));
        }


        private void ReceiveOpenDividendeProStueckAnpassenMessage(OpenDividendeProStueckAnpassenMessage m)
        {
            var View = new DividendeProStueckAnpassenView()
            {
                Owner = Application.Current.MainWindow
            };

            if (View.DataContext is DividendeProStueckAnpassenViewModel model)
            {
                model.LoadData(m.DividendeID, m.Umrechnungskurs);
            }

            View.ShowDialog();
        }


        private void ReceiveOpenDividendeAuswahlMessage(OpenDividendenAuswahlMessage m)
        {
            DividendenAuswahlView view = new DividendenAuswahlView()
            {
                Owner = Application.Current.MainWindow
            };
            if (view.DataContext is DividendenAuswahlViewModel model)
            {
                model.OhneHinterlegteDividende = true;
                model.LoadData( m.WertpapierID );

                _ = view.ShowDialog();

                if (model.AuswahlGetaetigt && model.ID().HasValue)
                {
                    m.Callback(true, model.ID().Value, model.Betrag(), model.Zahldatum());
                }
                else
                {
                    m.Callback(false, 0, 0 , DateTime.Now);
                }
            }
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
            base.Window_Unloaded(sender,e);
            Messenger.Default.Unregister<OpenDividendenAuswahlMessage>(this);
            Messenger.Default.Unregister<OpenDividendeProStueckAnpassenMessage>(this);
            Messenger.Default.Unregister<OpenSteuernUebersichtMessage>(this);
        }
    }
}
