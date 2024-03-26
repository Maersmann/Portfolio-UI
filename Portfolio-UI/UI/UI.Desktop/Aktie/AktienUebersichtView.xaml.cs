using CommunityToolkit.Mvvm.Messaging;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DepotMessages;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.AktieViewModels;
using Aktien.Logic.UI.DepotViewModels;
using Aktien.Logic.UI.DividendeViewModels;
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
using Aktien.UI.Desktop.Aktie;
using Aktien.UI.Desktop.Depot;
using Aktien.UI.Desktop.Dividende;
using Aktien.Logic.Messages.AktieMessages;
using UI.Desktop.Base;
using System.Security.Principal;
using Logic.Messages.UtilMessages;

namespace UI.Desktop.Aktie
{
    /// <summary>
    /// Interaktionslogik für AktienUebersichtView.xaml
    /// </summary>
    public partial class AktienUebersichtView : BaseUsercontrol
    {
        private string token;
        public AktienUebersichtView()
        {
            InitializeComponent();
            RegisterMessages("AktienUebersicht");
        }

        public string MessageToken
        {
            set
            {
                if (DataContext is AktienUebersichtViewModel modelUebersicht)
                {
                    token = value;
                    WeakReferenceMessenger.Default.Register<OpenDividendenUebersichtAuswahlMessage, string>(this, value , (r,m) => ReceiveOpenDividendeUebersichtMessage(m));
                    modelUebersicht.MessageToken = value;
                }
            }
        }

        private static void ReceiveOpenDividendeUebersichtMessage(OpenDividendenUebersichtAuswahlMessage m)
        {
            DividendenUebersichtAuswahlView view = new();

            if (view.DataContext is DividendenUebersichtAuswahlViewModel model)
            {
                model.WertpapierID = m.WertpapierID;
            }

            _ = view.ShowDialog();
        }

        protected override void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            base.Window_Unloaded(sender, e);
            WeakReferenceMessenger.Default.Unregister<OpenDividendenUebersichtAuswahlMessage, string>(this, token);
        }
    }
}
