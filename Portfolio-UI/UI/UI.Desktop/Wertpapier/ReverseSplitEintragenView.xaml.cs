using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
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
            Messenger.Default.Register<OpenWertpapierAuswahlMessage>(this, "ReverseSplitEintragen", m => ReceiveOpenWertpapierAuswahlMessage(m));
            Messenger.Default.Register<CloseViewMessage>(this, "ReverseSplitEintragen", m => ReceivCloseViewMessage());
            
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
                model.SetCallback(m.Callback);
                view.ShowDialog();
            }
        }
    }
}
