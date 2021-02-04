using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.UI.AuswahlViewModels;
using Aktien.Logic.UI.DividendeViewModels;
using Aktien.UI.Desktop.Auswahl;
using Aktien.UI.Desktop.Base;
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

namespace Aktien.UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendeErhaltenView.xaml
    /// </summary>
    public partial class DividendeErhaltenView : StammdatenView
    {
        private DividendenAuswahlView view;
        public DividendeErhaltenView()
        {
            InitializeComponent();
            base.MessageWithToken("ErhalteneDividendeStammdaten");
            Messenger.Default.Register<OpenDividendenAuswahlMessage>(this, m => ReceiveOpenDividendeAuswahlMessage(m));
            Messenger.Default.Register<DividendeAusgewaehltMessage>(this, m => ReceiveDividendeAusgewaehltMessage(m));
        }

        private void ReceiveDividendeAusgewaehltMessage(DividendeAusgewaehltMessage m)
        {
            view.Close();
            if (this.DataContext is DividendeErhaltenViewModel model)
            {
                model.DividendeAusgewaehlt(m.ID, m.Betrag, m.Datum);
            }
        }

        private void ReceiveOpenDividendeAuswahlMessage(OpenDividendenAuswahlMessage m)
        {
            view = new DividendenAuswahlView();
            if (view.DataContext is DividendenAuswahlViewModel model)
            {
                model.LoadData( m.WertpapierID );
            }
            
            view.ShowDialog();

        }

        public override void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            base.Window_Unloaded(sender,e);
            Messenger.Default.Unregister<OpenDividendenAuswahlMessage>(this);
            Messenger.Default.Unregister<DividendeAusgewaehltMessage>(this);
        }
    }
}
