using Aktien.UI.Desktop.Wertpapier;
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

namespace Aktien.UI.Desktop.ETF
{
    /// <summary>
    /// Interaktionslogik für ETFUebersichtPage.xaml
    /// </summary>
    public partial class ETFUebersichtPage : Page
    {
        public ETFUebersichtPage()
        {
            InitializeComponent();
            ContainerLeft.NavigationService.Navigate(new ETFGesamtUebersichtView { MessageToken = "ETFUebersicht" });
            ContainerRight.NavigationService.Navigate(new OrderUebersichtView { MessageToken = "ETFUebersicht" });
        }
    }
}
