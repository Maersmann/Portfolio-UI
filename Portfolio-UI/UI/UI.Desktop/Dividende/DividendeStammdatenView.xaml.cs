using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
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

namespace Aktien.UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendeStammdatenView.xaml
    /// </summary>
    public partial class DividendeStammdatenView : StammdatenView
    {
        public DividendeStammdatenView()
        {
            InitializeComponent();
            base.RegisterStammdatenGespeichertMessage(Data.Types.StammdatenTypes.dividende);
        }

    }
}
