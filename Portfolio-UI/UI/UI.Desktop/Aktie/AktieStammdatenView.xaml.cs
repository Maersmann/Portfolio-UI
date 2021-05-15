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
using System.Windows.Shapes;
using Aktien.UI.Desktop.Base;

namespace Aktien.UI.Desktop.Aktie
{
    /// <summary>
    /// Interaktionslogik für NeueAktieView.xaml
    /// </summary>
    public partial class AktieStammdatenView : StammdatenView
    {
        public AktieStammdatenView()
        {
            InitializeComponent();
            base.RegisterStammdatenGespeichertMessage(Data.Types.StammdatenTypes.aktien);
        }

    }
}
