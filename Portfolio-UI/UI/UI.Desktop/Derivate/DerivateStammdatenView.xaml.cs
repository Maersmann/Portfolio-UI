using Aktien.UI.Desktop.Base;
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
    /// Interaktionslogik für DerivateStammdatenView.xaml
    /// </summary>
    public partial class DerivateStammdatenView : StammdatenView
    {
        public DerivateStammdatenView()
        {
            InitializeComponent();
            RegisterStammdatenGespeichertMessage(Data.Types.StammdatenTypes.derivate);
        }
    }
}
