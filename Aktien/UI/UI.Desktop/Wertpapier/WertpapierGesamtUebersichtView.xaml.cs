using Aktien.Logic.UI.WertpapierViewModels;
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

namespace Aktien.UI.Desktop.Wertpapier
{
    /// <summary>
    /// Interaktionslogik für WertpapierGesamtView.xaml
    /// </summary>
    public partial class WertpapierGesamtUebersichtView : UserControl
    {
        public WertpapierGesamtUebersichtView()
        {
            InitializeComponent();
        }

        public string MessageToken
        {
            set
            {
                if (this.DataContext is WertpapierGesamtUebersichtViewModel modelUebersicht)
                {
                    modelUebersicht.MessageToken = value;
                }
            }
        }
    }
}
