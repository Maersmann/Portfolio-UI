using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.UI.AuswahlViewModels;
using Aktien.UI.Desktop.Auswahl;
using CommunityToolkit.Mvvm.Messaging;
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
    public partial class DividendeWertpapierEntwicklungView : UserControl
    {
        public DividendeWertpapierEntwicklungView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<OpenWertpapierAuswahlMessage, string>(this, "DividendeWertpapierEntwicklung", (r,m) => ReceiveOpenWertpapierAuswahlMessage(m));
        }

        private static void ReceiveOpenWertpapierAuswahlMessage(OpenWertpapierAuswahlMessage m)
        {
            WertpapierAuswahlView view = new()
            {
                Owner = Application.Current.MainWindow
            };
            
            if (view.DataContext is WertpapierAuswahlViewModel model)
            {
                model.SetTyp(Aktien.Data.Types.WertpapierTypes.WertpapierTypes.none);
                _ = view.ShowDialog();
                if (model.AuswahlGetaetigt && model.ID().HasValue)
                {
                    m.Callback(true, model.ID().Value);
                }
                else
                {
                    m.Callback(false, 0);
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<OpenWertpapierAuswahlMessage, string>(this, "DividendeWertpapierEntwicklung");
        }
    }
}