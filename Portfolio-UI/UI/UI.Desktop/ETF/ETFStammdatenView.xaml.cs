using UI.Desktop.Base;
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
using Aktien.Data.Types;

namespace UI.Desktop.ETF
{
    /// <summary>
    /// Interaktionslogik für ETFStammdatenView.xaml
    /// </summary>
    public partial class ETFStammdatenView : StammdatenView
    {
        public ETFStammdatenView()
        {
            InitializeComponent();
            RegisterStammdatenGespeichertMessage(StammdatenTypes.etf);
        }
    }
}
