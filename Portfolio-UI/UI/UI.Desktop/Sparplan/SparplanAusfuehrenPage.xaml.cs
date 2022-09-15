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

namespace UI.Desktop.Sparplan
{
    /// <summary>
    /// Interaktionslogik für SparplanAusfuehrenPage.xaml
    /// </summary>
    public partial class SparplanAusfuehrenPage : Page
    {
        public SparplanAusfuehrenPage()
        {
            InitializeComponent();
            ContainerLeft.NavigationService.Navigate(new SparplanAusfuehrenUebersichtView());
            ContainerRight.NavigationService.Navigate(new SparplanHistoryUebersichtView());
        }
    }
}
