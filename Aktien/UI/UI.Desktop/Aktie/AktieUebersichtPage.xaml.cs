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

namespace Aktien.UI.Desktop.Aktie
{
    /// <summary>
    /// Interaktionslogik für AktieUebersichtPage.xaml
    /// </summary>
    public partial class AktieUebersichtPage : Page
    {
        public AktieUebersichtPage()
        {
            InitializeComponent();
            ContainerLeft.NavigationService.Navigate(new AktienUebersichtView());
            ContainerRight.NavigationService.Navigate(new OrderUebersichtView());
        }
    }
}
