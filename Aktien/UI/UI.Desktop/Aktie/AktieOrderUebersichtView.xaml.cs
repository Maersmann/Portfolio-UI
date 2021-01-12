using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.DepotMessages;
using Aktien.Logic.UI.DepotViewModels;
using Aktien.UI.Desktop.Depot;
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

namespace Aktien.UI.Desktop.Aktie
{
    /// <summary>
    /// Interaktionslogik für AktieOrderUebersichtView.xaml
    /// </summary>
    public partial class AktieOrderUebersichtView : UserControl
    {
        public AktieOrderUebersichtView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenAktieGekauftViewMessage>(this, m => ReceiveOpenAktieGekauftViewMessage(m));
        }

        private void ReceiveOpenAktieGekauftViewMessage(OpenAktieGekauftViewMessage m)
        {
           
            var view = new BuyOrderView();
            if (view.DataContext is BuyOrderViewModel model)
            {
                model.AktieID = m.AktieID;
            }
            view.ShowDialog();
            Messenger.Default.Send<LoadAktieOrderMessage>(new LoadAktieOrderMessage { AktieID = m.AktieID });

        }
    }
}
