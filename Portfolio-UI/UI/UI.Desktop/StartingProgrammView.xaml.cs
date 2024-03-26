using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.SteuernMessages;
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
using System.Windows.Shapes;

namespace Aktien.UI.Desktop
{
    /// <summary>
    /// Interaktionslogik für StartingProgrammView.xaml
    /// </summary>
    public partial class StartingProgrammView : Window
    {
        public StartingProgrammView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<CloseViewMessage, string>(this, "StartingProgramm", (r,m)=> ReceivCloseViewMessage());
        }

        private void ReceivCloseViewMessage()
        {
            Window.GetWindow(this).Close();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<CloseViewMessage, string>(this, "StartingProgramm");

        }
    }
}
