using Logic.UI.SteuerViewModels;
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

namespace UI.Desktop.Steuer
{
    /// <summary>
    /// Interaktionslogik für SteuerartenUebersichtView.xaml
    /// </summary>
    public partial class SteuerartenUebersichtView : UserControl
    {
        public SteuerartenUebersichtView()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool visible)
            { 
                if (visible)
                { 
                    if (this.DataContext is SteuerartenUebersichtViewModel modelUebersicht)
                        modelUebersicht.LoadData();
                }
            }

        }
    }
}
