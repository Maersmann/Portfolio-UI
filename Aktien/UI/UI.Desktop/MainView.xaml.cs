using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
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
using UI.Desktop.Aktie;

namespace UI.Desktop
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenNeueAktieViewMessage>(this, m => ReceiveOpenNeueAktieViewMessage());
        }

        private void Container_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // if (e.Content == _formMatch)
            // {
            //     ribboncontextMatch.Visibility = Visibility.Visible;
            //     ribbonMatch.IsSelected = true;
            // }
            // else
            //     ribboncontextMatch.Visibility = Visibility.Hidden;
        }

        private void ReceiveOpenNeueAktieViewMessage()
        {
            var view = new NeueAktieView();
            view.ShowDialog();
        }

    }

}
