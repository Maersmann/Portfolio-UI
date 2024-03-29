﻿using Aktien.UI.Desktop.Wertpapier;
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

namespace Aktien.UI.Desktop.Depot
{
    /// <summary>
    /// Interaktionslogik für DepotUebersichtPage.xaml
    /// </summary>
    public partial class DepotUebersichtPage : Page
    {
        public DepotUebersichtPage()
        {
            InitializeComponent();
            ContainerLeft.NavigationService.Navigate(new DepotUebersichtView { MessageToken = "DepotUebersicht" });
            ContainerRight.NavigationService.Navigate(new OrderUebersichtView { MessageToken = "DepotUebersicht" });
        }
    }
}
