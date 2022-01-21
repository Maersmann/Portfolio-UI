using Aktien.Data.Types;
using Aktien.UI.Desktop.Base;
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
using System.Windows.Shapes;

namespace UI.Desktop.User
{
    /// <summary>
    /// Interaktionslogik für VorbelegungView.xaml
    /// </summary>
    public partial class VorbelegungView : StammdatenView
    {
        public VorbelegungView()
        {
            InitializeComponent();
            RegisterStammdatenGespeichertMessage(StammdatenTypes.vorbelegung);
        }
    }
}
