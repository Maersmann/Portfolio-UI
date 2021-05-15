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

namespace Aktien.UI.Desktop.Depot
{
    /// <summary>
    /// Interaktionslogik für EinahmenAusgabenUebersichtPage.xaml
    /// </summary>
    public partial class EinnahmenAusgabenUebersichtPage : Page
    {
        public EinnahmenAusgabenUebersichtPage()
        {
            InitializeComponent();
            ContainerLeft.NavigationService.Navigate(new EinnahmenUebersichtView { MessageToken = "EinnahmenAusgabenUebersicht" });
            ContainerRight.NavigationService.Navigate(new AusgabenUebersichtView { MessageToken = "EinnahmenAusgabenUebersicht" });
            ContainerUp.NavigationService.Navigate(new EinnahmenAusgabenGesamtUebersichtView());
        }
    }
}
