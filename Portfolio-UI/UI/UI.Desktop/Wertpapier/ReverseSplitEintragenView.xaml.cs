using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
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

namespace Aktien.UI.Desktop.Wertpapier
{
    /// <summary>
    /// Interaktionslogik für ReverseSplitEintragenView.xaml
    /// </summary>
    public partial class ReverseSplitEintragenView : UserControl
    {
        public ReverseSplitEintragenView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<OpenWertpapierAuswahlMessage, string>(this, "ReverseSplitEintragen", (r,m) => ReceiveOpenWertpapierAuswahlMessage(m));
            WeakReferenceMessenger.Default.Register<CloseViewMessage, string>(this, "ReverseSplitEintragen", (r, m) => ReceivCloseViewMessage());
            
        }

        private void ReceivCloseViewMessage()
        {
            Window.GetWindow(this).Close();
        }

        private void ReceiveOpenWertpapierAuswahlMessage(OpenWertpapierAuswahlMessage m)
        {
            var view = new WertpapierAuswahlView()
            {
                Owner = Application.Current.MainWindow
            };
            
            if (view.DataContext is WertpapierAuswahlViewModel model)
            {
                model.SetTyp(m.WertpapierTypes);
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

        private void ReverseSplit_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<CloseViewMessage, string>(this, "ReverseSplitEintragen");
            WeakReferenceMessenger.Default.Unregister<OpenWertpapierAuswahlMessage, string>(this, "ReverseSplitEintragen");

        }
    }
}
