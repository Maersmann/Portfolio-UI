using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.DepotMessages;
using Aktien.Logic.UI.AktieViewModels;
using Aktien.Logic.UI.DepotViewModels;
using Aktien.Logic.UI.WertpapierViewModels;
using Aktien.UI.Desktop.Depot;
using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.UtilMessages;
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
using UI.Desktop.Depot;

namespace UI.Desktop.Wertpapier
{
    /// <summary>
    /// Interaktionslogik für AktieOrderUebersichtView.xaml
    /// </summary>
    public partial class OrderUebersichtView : BaseUsercontrol
    {
        string token;
        public OrderUebersichtView()
        {
            InitializeComponent();
            RegisterMessages("OrderUebersicht");
        }

        public string MessageToken
        {
            set
            {
                token = value;
                WeakReferenceMessenger.Default.Register<OpenWertpapierGekauftViewMessage, string>(this, value, (r,m) => ReceiveOpenAktieGekauftViewMessage(m));
                if (this.DataContext is OrderUebersichtViewModel modelUebersicht)
                {
                    modelUebersicht.MessageToken = value;
                }
            }
        }


        private static void ReceiveOpenAktieGekauftViewMessage(OpenWertpapierGekauftViewMessage m)
        {

            var view = new BuyOrderView()
            {
                Owner = Application.Current.MainWindow
            };
            if (view.DataContext is BuyOrderViewModel model)
            {
                model.WertpapierID = m.WertpapierID;
                model.SetTitle(m.BuySell, m.WertpapierTypes);
            }
            view.ShowDialog();

        }

        protected override void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            base.Window_Unloaded(sender, e);
            WeakReferenceMessenger.Default.Unregister<OpenWertpapierGekauftViewMessage, string>(this, token);
        }
    }
}
