using Aktien.Data.Types;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.AuswahlViewModels;
using Aktien.Logic.UI.DividendeViewModels;
using Aktien.UI.Desktop.Auswahl;
using Aktien.UI.Desktop.Dividende;
using Data.Types.SteuerTypes;
using CommunityToolkit.Mvvm.Messaging;
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
using UI.Desktop.Base;
using UI.Desktop.Steuer;

namespace UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendeErhaltenView.xaml
    /// </summary>
    public partial class DividendeErhaltenView : StammdatenView
    {
        public DividendeErhaltenView()
        {
            InitializeComponent();
            RegisterStammdatenGespeichertMessage(StammdatenTypes.dividendeErhalten);
            WeakReferenceMessenger.Default.Register<OpenDividendenAuswahlMessage, string>(this, "DividendeErhalten", (r, m) => ReceiveOpenDividendeAuswahlMessage(m));
            WeakReferenceMessenger.Default.Register<OpenSteuernUebersichtMessage, string>(this, "DividendeErhalten", (r, m) => ReceiveOpenSteuernUebersichtMessage(m));
            WeakReferenceMessenger.Default.Register<OpenDividendeProStueckAnpassenMessage, string>(this, "DividendeErhalten",(r, m) => ReceiveOpenDividendeProStueckAnpassenMessage(m));
        }


        private static void ReceiveOpenDividendeProStueckAnpassenMessage(OpenDividendeProStueckAnpassenMessage m)
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


        private static void ReceiveOpenDividendeAuswahlMessage(OpenDividendenAuswahlMessage m)
        {
            DividendenAuswahlView view = new()
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

        private static void ReceiveOpenSteuernUebersichtMessage(OpenSteuernUebersichtMessage m)
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
            base.Window_Unloaded(sender,e);
            WeakReferenceMessenger.Default.Unregister<OpenDividendenAuswahlMessage, string>(this, "DividendeErhalten");
            WeakReferenceMessenger.Default.Unregister<OpenDividendeProStueckAnpassenMessage, string>(this, "DividendeErhalten");
            WeakReferenceMessenger.Default.Unregister<OpenSteuernUebersichtMessage, string>(this, "DividendeErhalten");
        }
    }
}
