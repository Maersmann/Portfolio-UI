using Aktien.Logic.Messages.Base;
using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.SparplanMessages;
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
using System.Windows.Shapes;

namespace UI.Desktop.Sparplan
{
    /// <summary>
    /// Interaktionslogik für SparplanAusfuehrenView.xaml
    /// </summary>
    public partial class SparplanAusfuehrenView : Window
    {
        public SparplanAusfuehrenView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<CloseViewMessage, string>(this, "SparplanAusfuehren", (r,m) => ReceivCloseViewMessage());
        }
        private void ReceivCloseViewMessage()
        {
            GetWindow(this).Close();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<CloseViewMessage, string>(this, "SparplanAusfuehren");

        }
    }
}
