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
using Aktien.UI.Desktop.Optionen;
using UI.Desktop.Steuer;
using Logic.Messages.Base;
using UI.Desktop.Konfigruation;
using UI.Desktop.Auswertung;

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
            Messenger.Default.Register<OpenStartingViewMessage>(this, m => ReceiceOpenStartingViewMessage());
            Messenger.Default.Register<OpenKonfigurationViewMessage>(this, m => ReceiceOpenKonfigurationViewMessage());
            Messenger.Default.Register<CloseApplicationMessage>(this, m => ReceiceCloseApplicationMessage());
            

            DatenAnpassungFrame.Navigate(new DatenAnpassungView());
        }

        private void ReceiveInformationMessage(InformationMessage m)
        {
            MessageBox.Show(m.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ReceiveExceptionMessage(ExceptionMessage m)
        {
            MessageBox.Show(m.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    aktienUebersichtView ??= new AktieUebersichtPage();
                    Container.NavigationService.Navigate(aktienUebersichtView);
                    break;
                case ViewType.viewAktieGekauft:
                    new BuyOrderView().ShowDialog();
                    break;
                case ViewType.viewDepotUebersicht:
                    depotUebersichtView ??= new DepotUebersichtPage();
                    Container.NavigationService.Navigate(depotUebersichtView);
                    break;
                case ViewType.viewETFUebersicht:
                    etfGesamtUebersicht ??= new ETFUebersichtPage();
                    Container.NavigationService.Navigate(etfGesamtUebersicht);
                    break;
                case ViewType.viewWertpapierUebersicht:
                    wertpapierGesamtUebersichtPage ??= new WertpapierGesamtUebersichtPage();
                    Container.NavigationService.Navigate(wertpapierGesamtUebersichtPage);
                    break;
                case ViewType.viewDerivateUebersicht:
                    derivateGesamtUebersichtPage ??= new DerivateUebersichtPage();
                    Container.NavigationService.Navigate(derivateGesamtUebersichtPage);
                    break;
                case ViewType.viewEinAusgabenUebersicht:
                    EinnahmenAusgabenUebersichtPage ??= new EinnahmenAusgabenUebersichtPage();
                    Container.NavigationService.Navigate(EinnahmenAusgabenUebersichtPage);
                    break;
                case ViewType.viewDivideneMonatAuswertung:
                    Container.NavigationService.Navigate(new DivideneMonatAuswertungView());
                    break;
                case ViewType.viewDivideneMonatJahresauswertungAuswertung:
                    Container.NavigationService.Navigate(new DividendeMonatJahresVergleichAuswertungView());
                    break;
                case ViewType.viewSteuerartMonatAuswertung:
                    Container.NavigationService.Navigate(new SteuerartMonatAuswertungView());
                    break;
                case ViewType.viewSteuerMonatJahresAuswertung:
                    Container.NavigationService.Navigate(new SteuerMonatJahresVergleichAuswertungView());  
                    break;
                case ViewType.viewDivideneWertpapierAuswertung:
                    Container.NavigationService.Navigate(new DividendeWertpapierAuswertungView());
                    break;
                case ViewType.viewSteuerMonatAuswertung:
                    Container.NavigationService.Navigate(new SteuerMonatAuswertungView());
                    break;
                case ViewType.viewOpenDividendeWertpapierEntwicklungAuswertung:
                    Container.NavigationService.Navigate(new DividendeWertpapierEntwicklungAuswertungView());
                    break;                  
                default:
                    break;

                    
            }
        }


        private void ReceiceOpenStammdatenMessage(BaseStammdatenMessage m)
        {
            StammdatenView view = null;
            switch (m.StammdatenTyp)
            {
                case StammdatenTypes.aktien:
                    view = new AktieStammdatenView();
                    break;
                case StammdatenTypes.etf:
                    view = new ETFStammdatenView();
                    break;
                case StammdatenTypes.derivate:
                    view = new DerivateStammdatenView();
                    break;
                case StammdatenTypes.ausgaben:
                    view = new AusgabeStammdatenView();
                    break;
                case StammdatenTypes.einnahmen:
                    view = new EinnahmeStammdatenView();
                    break;
                case StammdatenTypes.steuerart:
                    view = new SteuerartStammdatenView();
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
            view.Owner = this;
            view.ShowDialog();
        }

        private void ReceiceOpenStartingViewMessage()
        {
            var view = new StartingProgrammView();
            view.ShowDialog();
        }

        private void ReceiceOpenKonfigurationViewMessage()
        {
            new KonfigurationView().ShowDialog();
        }

        private void ReceiceCloseApplicationMessage()
        {
            Application.Current.Shutdown();
        }

    }

}
