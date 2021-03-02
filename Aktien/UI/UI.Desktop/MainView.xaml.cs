using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.Base;
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
using Aktien.Data.Types;
using Aktien.UI.Desktop.Depot;
using Aktien.Logic.Messages;
using Aktien.UI.Desktop.ETF;
using Aktien.UI.Desktop.Wertpapier;
using Aktien.UI.Desktop.Derivate;
using Aktien.UI.Desktop.Base;
using Aktien.Logic.UI.InterfaceViewModels;
using Aktien.UI.Desktop.Dividende;

namespace Aktien.UI.Desktop
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainView
    {

        private static AktieUebersichtPage aktienUebersichtView;
        private static DepotUebersichtPage depotUebersichtView;
        private static ETFUebersichtPage etfGesamtUebersicht;
        private static WertpapierGesamtUebersichtPage wertpapierGesamtUebersichtPage;
        private static DerivateUebersichtPage derivateGesamtUebersichtPage;
        private static EinnahmenAusgabenUebersichtPage EinnahmenAusgabenUebersichtPage;

        public MainView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenViewMessage>(this, m => ReceiveOpenViewMessage(m));
            Messenger.Default.Register<ExceptionMessage>(this, m => ReceiveExceptionMessage(m));
            Messenger.Default.Register<InformationMessage>(this, m => ReceiveInformationMessage(m));
            Messenger.Default.Register<BaseStammdatenMessage>(this, m => ReceiceOpenStammdatenMessage(m));

            Naviagtion(ViewType.viewWertpapierUebersicht);
        }

        private void ReceiveInformationMessage(InformationMessage m)
        {
            MessageBox.Show(m.Message);
        }

        private void ReceiveExceptionMessage(ExceptionMessage m)
        {
            MessageBox.Show(m.Message);
        }

        private void ReceiveOpenViewMessage(OpenViewMessage m)
        {
            Naviagtion(m.ViewType);
        }

        public void Naviagtion(ViewType inType)
        {
            switch (inType)
            {
                case ViewType.viewAktienUebersicht:
                    aktienUebersichtView = aktienUebersichtView ?? new AktieUebersichtPage();
                    Container.NavigationService.Navigate(aktienUebersichtView);
                    break;
                case ViewType.viewAktieGekauft:
                    new BuyOrderView().ShowDialog();
                    break;
                case ViewType.viewDepotUebersicht:
                    depotUebersichtView = depotUebersichtView ?? new DepotUebersichtPage();
                    Container.NavigationService.Navigate(depotUebersichtView);
                    break;
                case ViewType.viewETFUebersicht:
                    etfGesamtUebersicht = etfGesamtUebersicht ?? new ETFUebersichtPage();
                    Container.NavigationService.Navigate(etfGesamtUebersicht);
                    break;
                case ViewType.viewWertpapierUebersicht:
                    wertpapierGesamtUebersichtPage = wertpapierGesamtUebersichtPage ?? new WertpapierGesamtUebersichtPage();
                    Container.NavigationService.Navigate(wertpapierGesamtUebersichtPage);
                    break;
                case ViewType.viewDerivateUebersicht:
                    derivateGesamtUebersichtPage = derivateGesamtUebersichtPage ?? new DerivateUebersichtPage();
                    Container.NavigationService.Navigate(derivateGesamtUebersichtPage);
                    break;
                case ViewType.viewEinAusgabenUebersicht:
                    EinnahmenAusgabenUebersichtPage = EinnahmenAusgabenUebersichtPage ?? new EinnahmenAusgabenUebersichtPage();
                    Container.NavigationService.Navigate(EinnahmenAusgabenUebersichtPage);
                    break;
                default:
                    break;
            }
        }


        private void ReceiceOpenStammdatenMessage(BaseStammdatenMessage m)
        {
            StammdatenView view = null;
            switch (m.ViewType)
            {
                case ViewType.viewAktieStammdaten:
                    view = new AktieStammdatenView();
                    break;
                case ViewType.viewETFStammdaten:
                    view = new ETFStammdatenView();
                    break;
                case ViewType.viewDerivateStammdaten:
                    view = new DerivateStammdatenView();
                    break;
                default:
                    break;
            }

            if (view.DataContext is IViewModelStammdaten model)
            {
                if (m.State == State.Bearbeiten)
                {
                    model.ZeigeStammdatenAn(m.ID.Value);
                }

            }
            view.ShowDialog();
        }
    }

}
