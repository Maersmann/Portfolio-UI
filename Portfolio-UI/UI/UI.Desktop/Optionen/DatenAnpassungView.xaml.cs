using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.UI.AuswahlViewModels;
using Aktien.UI.Desktop.Auswahl;
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
            WeakReferenceMessenger.Default.Register<OpenWertpapierAuswahlMessage, string>(this, "DatenAnpassung", (r,m) => ReceiveOpenWertpapierAuswahlMessage(m));
        }

        private void ReceiveOpenWertpapierAuswahlMessage(OpenWertpapierAuswahlMessage m)
        {
            var view = new WertpapierAuswahlView()
            {
                Owner = Application.Current.MainWindow
            };
            
            if (view.DataContext is WertpapierAuswahlViewModel model)
            {
                model.SetTyp(Data.Types.WertpapierTypes.WertpapierTypes.none);
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
            WeakReferenceMessenger.Default.Unregister<OpenWertpapierAuswahlMessage, string>(this, "DatenAnpassung");
        }
    }
}
