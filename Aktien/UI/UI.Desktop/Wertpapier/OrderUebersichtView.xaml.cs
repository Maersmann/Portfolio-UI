using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.DepotMessages;
using Aktien.Logic.UI.AktieViewModels;
using Aktien.Logic.UI.DepotViewModels;
using Aktien.Logic.UI.WertpapierViewModels;
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

namespace Aktien.UI.Desktop.Wertpapier
{
    /// <summary>
    /// Interaktionslogik für AktieOrderUebersichtView.xaml
    /// </summary>
    public partial class OrderUebersichtView : UserControl
    {
        public OrderUebersichtView()
        {
            InitializeComponent();
            
        }

        public string MessageToken
        {
            set
            {
                Messenger.Default.Register<OpenAktieGekauftViewMessage>(this, value, m => ReceiveOpenAktieGekauftViewMessage(m));
                if (this.DataContext is OrderUebersichtViewModel modelUebersicht)
                {
                    modelUebersicht.MessageToken = value;
                }
            }
        }


        private void ReceiveOpenAktieGekauftViewMessage(OpenAktieGekauftViewMessage m)
        {
           
            var view = new BuyOrderView();
            if (view.DataContext is BuyOrderViewModel model)
            {
                model.WertpapierID = m.WertpapierID;
                model.SetTitle(m.BuySell, m.WertpapierTypes);
            }
            view.ShowDialog();

        }
    }
}
