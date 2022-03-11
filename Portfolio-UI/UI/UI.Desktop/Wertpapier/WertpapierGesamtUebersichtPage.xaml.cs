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
using UI.Desktop.Wertpapier;

namespace Aktien.UI.Desktop.Wertpapier
{
    /// <summary>
    /// Interaktionslogik für WertpapierGesamtUebersichtPage.xaml
    /// </summary>
    public partial class WertpapierGesamtUebersichtPage : Page
    {
        public WertpapierGesamtUebersichtPage()
        {
            InitializeComponent();
            ContainerLeft.NavigationService.Navigate(new WertpapierGesamtUebersichtView { MessageToken = "WertpapierGesamtUebersicht" });
            ContainerRight.NavigationService.Navigate(new OrderUebersichtView { MessageToken = "WertpapierGesamtUebersicht" });
        }
    }
}
