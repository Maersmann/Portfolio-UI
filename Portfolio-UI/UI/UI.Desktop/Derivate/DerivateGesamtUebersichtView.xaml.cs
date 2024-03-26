using Aktien.Logic.UI.DerivateViewModels;
using CommunityToolkit.Mvvm.Messaging;
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

namespace Aktien.UI.Desktop.Derivate
{
    /// <summary>
    /// Interaktionslogik für DerivateGesamtUebersichtView.xaml
    /// </summary>
    public partial class DerivateGesamtUebersichtView : UserControl
    {
        public DerivateGesamtUebersichtView()
        {
            InitializeComponent();
        }

        public string MessageToken
        {
            set
            {
                if (this.DataContext is DerivateGesamtUebersichtViewModel modelUebersicht)
                {
                    modelUebersicht.MessageToken = value;
                }
            }
        }

    }
}