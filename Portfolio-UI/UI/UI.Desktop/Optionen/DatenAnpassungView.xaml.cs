using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.UI.AuswahlViewModels;
using Aktien.UI.Desktop.Auswahl;
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

namespace Aktien.UI.Desktop.Optionen
{
    /// <summary>
    /// Interaktionslogik für DatenAnpassungView.xaml
    /// </summary>
    public partial class DatenAnpassungView : UserControl
    {
        public DatenAnpassungView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenWertpapierAuswahlMessage>(this, "DatenAnpassung", m => ReceiveOpenWertpapierAuswahlMessage(m));
        }

        private void ReceiveOpenWertpapierAuswahlMessage(OpenWertpapierAuswahlMessage m)
        {
            var view = new WertpapierAuswahlView();
            if (view.DataContext is WertpapierAuswahlViewModel model)
            {
                model.SetCallback(m.Callback);
                model.LoadData();         
            }
            view.ShowDialog();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<OpenWertpapierAuswahlMessage>(this);
        }
    }
}
