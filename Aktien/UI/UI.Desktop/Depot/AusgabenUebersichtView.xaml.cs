using Aktien.Logic.Messages.AusgabenMessages;
using Aktien.Logic.UI.DepotViewModels;
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

namespace Aktien.UI.Desktop.Depot
{
    /// <summary>
    /// Interaktionslogik für AusgabenUebersichtVierw.xaml
    /// </summary>
    public partial class AusgabenUebersichtView : UserControl
    {
        public AusgabenUebersichtView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenAusgabeStammdatenMessage>(this, m => ReceivOpenAusgabeStammdatenMessage(m));
        }

        public string MessageToken
        {
            set
            {

                if (this.DataContext is AusgabenUebersichtViewModel modelUebersicht)
                {
                    modelUebersicht.MessageToken = value;
                }
            }
        }

        private void ReceivOpenAusgabeStammdatenMessage(OpenAusgabeStammdatenMessage m)
        {
            new AusgabeStammdatenView().ShowDialog();
        }
    }
}
