using LiveCharts;
using LiveCharts.Wpf;
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

namespace UI.Desktop.Auswertung
{
    /// <summary>
    /// Interaktionslogik für DividendeWertpapierAuswertungView.xaml
    /// </summary>
    public partial class DividendeWertpapierAuswertungView : UserControl
    {
        public DividendeWertpapierAuswertungView()
        {
            InitializeComponent();
        }

        private void PieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show(chartPoint.Y +"€ Dividende von " + chartPoint.SeriesView.Title);
        }
    }
}
