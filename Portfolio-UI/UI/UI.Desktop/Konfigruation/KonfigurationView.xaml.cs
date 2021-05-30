using Aktien.UI.Desktop.Optionen;
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

namespace UI.Desktop.Konfigruation
{
    /// <summary>
    /// Interaktionslogik für KonfigurationView.xaml
    /// </summary>
    public partial class KonfigurationView : Window
    {
        public KonfigurationView()
        {
            InitializeComponent();
            Container.NavigationService.Navigate(new BackendSettingsView());
        }
    }
}
