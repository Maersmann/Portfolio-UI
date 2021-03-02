using Aktien.Logic.UI.ETFViewModels;
using GalaSoft.MvvmLight.Messaging;
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

namespace Aktien.UI.Desktop.ETF
{
    /// <summary>
    /// Interaktionslogik für ETFGesamtUebersicht.xaml
    /// </summary>
    public partial class ETFGesamtUebersichtView : UserControl
    {
       
        public ETFGesamtUebersichtView()
        {
            InitializeComponent();
        }

        public string MessageToken 
        { 
            set 
            {
                if (this.DataContext is ETFGesamtUebersichtViewModel modelUebersicht)
                {
                    modelUebersicht.MessageToken = value;
                }
            } 
        }
    }
}
