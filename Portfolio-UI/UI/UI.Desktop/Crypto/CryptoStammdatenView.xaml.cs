using Aktien.Data.Types;
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
using System.Windows.Shapes;
using UI.Desktop.Base;

namespace UI.Desktop.Crypto
{
    /// <summary>
    /// Interaktionslogik für CryptoStammdatenView.xaml
    /// </summary>
    public partial class CryptoStammdatenView : StammdatenView
    {
        public CryptoStammdatenView()
        {
            InitializeComponent();
            base.RegisterStammdatenGespeichertMessage(StammdatenTypes.crypto);
        }
    }
}
