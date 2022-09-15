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
using UI.Desktop.Base;

namespace UI.Desktop.Sparplan
{
    /// <summary>
    /// Interaktionslogik für SparplanUebersichtView.xaml
    /// </summary>
    public partial class SparplanUebersichtView : BaseUsercontrol
    {
        public SparplanUebersichtView()
        {
            InitializeComponent();
            RegisterMessages("SparplanUebersicht");
        }
    }
}
