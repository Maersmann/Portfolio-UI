using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.SparplanMessages;
using Logic.UI.SparplanViewModels;
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
using UI.Desktop.Base;

namespace UI.Desktop.Sparplan
{
    /// <summary>
    /// Interaktionslogik für SparplanAusfuehrenUebersichtView.xaml
    /// </summary>
    public partial class SparplanAusfuehrenUebersichtView : BaseUsercontrol
    {
        public SparplanAusfuehrenUebersichtView()
        {
            InitializeComponent();
            RegisterMessages("SparplanAusfuehrenUebersicht");
            Messenger.Default.Register<OpenSparplanAusfuehrenMessage>(this, "SparplanAusfuehrenUebersicht", m => ReceiveOpenSparplanAusfuehrenMessage(m));
        }

        private void ReceiveOpenSparplanAusfuehrenMessage(OpenSparplanAusfuehrenMessage m)
        {
            var view = new SparplanAusfuehrenView()
            {
                Owner = Application.Current.MainWindow
            };

            if (view.DataContext is SparplanAusfuehrenViewModel model)
            {
                model.SetzInformationen(m.SparplanAusfuehren);
                _ = view.ShowDialog();
            }
        }

        protected override void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            base.Window_Unloaded(sender, e);
            Messenger.Default.Unregister<OpenSparplanAusfuehrenMessage>(this);
        }
    }
}