using Aktien.Logic.Messages.Base;
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
using System.Windows.Shapes;

namespace UI.Desktop.Wertpapier
{
    /// <summary>
    /// Interaktionslogik für SplitEintragenView.xaml
    /// </summary>
    public partial class SplitEintragenView : Window
    {
        public SplitEintragenView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<CloseViewMessage, string>(this, "SplitEintragen", (r,m) => ReceivCloseViewMessage());
        }
        private void ReceivCloseViewMessage()
        {
            GetWindow(this).Close();
        }

        private void AktienSplit_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<CloseViewMessage, string>(this, "SplitEintragen");

        }
    }
}
