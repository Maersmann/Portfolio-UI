using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.UI.AuswahlViewModels;
using Aktien.UI.Desktop.Auswahl;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.Desktop.Auswertung
{
    /// <summary>
    /// Interaktionslogik für DividendeWertpapierEntwicklungView.xaml
    /// </summary>
    public partial class DividendeWertpapierEntwicklungAuswertungView : UserControl
    {
        public DividendeWertpapierEntwicklungAuswertungView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenWertpapierAuswahlMessage>(this, "DividendeWertpapierEntwicklung", m => ReceiveOpenWertpapierAuswahlMessage(m));
        }

        private void ReceiveOpenWertpapierAuswahlMessage(OpenWertpapierAuswahlMessage m)
        {
            var view = new WertpapierAuswahlView()
            {
                Owner = Application.Current.MainWindow
            };
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