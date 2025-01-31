using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.AktieViewModels;
using Aktien.Logic.UI.DividendeViewModels;
using Aktien.UI.Desktop.Dividende;
using CommunityToolkit.Mvvm.Messaging;
using Logic.UI.CryptoViewModels;
using Newtonsoft.Json.Linq;
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
using UI.Desktop.Base;

namespace UI.Desktop.Crypto
{
    /// <summary>
    /// Interaktionslogik für CryptoUebersichtView.xaml
    /// </summary>
    public partial class CryptoUebersichtView : BaseUsercontrol
    {
        private string token;

        public CryptoUebersichtView()
        {
            InitializeComponent();
            RegisterMessages("CryptoUebersicht");
        }

        public string MessageToken
        {
            set
            {
                if (DataContext is CryptoUebersichtViewModel modelUebersicht)
                {
                    token = value;
                    modelUebersicht.MessageToken = value;
                }
            }
        }


        protected override void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            base.Window_Unloaded(sender, e);
        }
    }
}
