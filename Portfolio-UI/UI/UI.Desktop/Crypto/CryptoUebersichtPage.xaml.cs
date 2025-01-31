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
using UI.Desktop.Wertpapier;

namespace UI.Desktop.Crypto
{
    /// <summary>
    /// Interaktionslogik für CryptoUebersichtPage.xaml
    /// </summary>
    public partial class CryptoUebersichtPage : Page
    {
        public CryptoUebersichtPage()
        {
            InitializeComponent();
            ContainerLeft.NavigationService.Navigate(new CryptoUebersichtView { MessageToken = "CryptoUebersicht" });
            ContainerRight.NavigationService.Navigate(new OrderUebersichtView { MessageToken = "CryptoUebersicht" });
        }
    }
}
