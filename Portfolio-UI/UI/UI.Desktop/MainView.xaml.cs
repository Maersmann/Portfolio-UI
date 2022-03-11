﻿using GalaSoft.MvvmLight.Messaging;
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
using UI.Desktop.Base;
using Aktien.Logic.UI.InterfaceViewModels;
using Aktien.UI.Desktop.Dividende;
using Aktien.UI.Desktop.Optionen;
using UI.Desktop.Steuer;
using Logic.Messages.Base;
using UI.Desktop.Konfigruation;
using UI.Desktop.Auswertung;
using Base.Logic.Messages;
using Base.Logic.Types;
using UI.Desktop.Auswertung.DividendeErhalten;
using UI.Desktop.Depot;
using UI.Desktop;
using UI.Desktop.Auswertung.Steuer;
using UI.Desktop.User;
using UI.Desktop.Auswertung.Investition;
using UI.Desktop.Aktie;
using UI.Desktop.ETF;
using UI.Desktop.Derivate;

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
            Messenger.Default.Register<BaseStammdatenMessage<StammdatenTypes>>(this, m => ReceiceOpenStammdatenMessage(m));
            Messenger.Default.Register<OpenStartingViewMessage>(this, m => ReceiceOpenStartingViewMessage());
            Messenger.Default.Register<OpenLoginViewMessage>(this, m => ReceiceOpenLoginViewMessage());
            Messenger.Default.Register<OpenKonfigurationViewMessage>(this, m => ReceiceOpenKonfigurationViewMessage());
            Messenger.Default.Register<CloseApplicationMessage>(this, m => ReceiceCloseApplicationMessage());
            

            DatenAnpassungFrame.Navigate(new DatenAnpassungView());
        }

        private void ReceiveInformationMessage(InformationMessage m)
        {
            _ = MessageBox.Show(m.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ReceiveExceptionMessage(ExceptionMessage m)
        {
            _ = MessageBox.Show(m.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(DividendeEntwicklungMonatlichView).Name))
                        Container.NavigationService.Navigate(new DividendeEntwicklungMonatlichView());
                    break;
                case ViewType.viewDivideneMonatJahresauswertungAuswertung:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(DividendeVergleichMonatView).Name))
                        Container.NavigationService.Navigate(new DividendeVergleichMonatView());
                    break;
                case ViewType.viewSteuerartMonatAuswertung:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(SteuerartMonatAuswertungView).Name))
                        Container.NavigationService.Navigate(new SteuerartMonatAuswertungView());
                    break;
                case ViewType.viewSteuerMonatJahresAuswertung:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(SteuerMonatJahresVergleichAuswertungView).Name))
                        Container.NavigationService.Navigate(new SteuerMonatJahresVergleichAuswertungView());
                    break;
                case ViewType.viewDivideneWertpapierAuswertung:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(DividendeWertpapierAuswertungView).Name))
                        Container.NavigationService.Navigate(new DividendeWertpapierAuswertungView());
                    break;
                case ViewType.viewSteuerMonatAuswertung:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(SteuerMonatAuswertungView).Name))
                        Container.NavigationService.Navigate(new SteuerMonatAuswertungView());
                    break;
                case ViewType.viewOpenDividendeWertpapierEntwicklungAuswertung:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(DividendeWertpapierEntwicklungView).Name))
                        Container.NavigationService.Navigate(new DividendeWertpapierEntwicklungView());
                    break;
                case ViewType.viewOpenDividendenErhaltenImJahr:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(DividendenErhaltenImJahrView).Name))
                        Container.NavigationService.Navigate(new DividendenErhaltenImJahrView());
                    break;
                case ViewType.viewOpenDividendenErhaltenImMonat:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(DividendenErhaltenImMonatView).Name))
                        Container.NavigationService.Navigate(new DividendenErhaltenImMonatView());
                    break;
                case ViewType.viewOpenOrderBuch:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(OrderBuchView).Name))
                        Container.NavigationService.Navigate(new OrderBuchView());
                    break;
                case ViewType.viewSteuerJahresgesamtbetragAuswertung:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(SteuerJahresgesamtbetragAuswertungView).Name))
                        Container.NavigationService.Navigate(new SteuerJahresgesamtbetragAuswertungView());
                    break;
                case ViewType.viewSteuerMonatgesamtbetragAuswertung:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(SteuerMonatgesamtbetragAuswertungView).Name))
                        Container.NavigationService.Navigate(new SteuerMonatgesamtbetragAuswertungView());
                    break;
                case ViewType.viewDividendeGesamtentwicklungSummiert:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(DividendeGesamtentwicklungSummiertView).Name))
                        Container.NavigationService.Navigate(new DividendeGesamtentwicklungSummiertView());
                    break;
                case ViewType.viewDividendeJahresentwicklungSummiert:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(DividendeJahresentwicklungSummiertView).Name))
                        Container.NavigationService.Navigate(new DividendeJahresentwicklungSummiertView());
                    break;
                case ViewType.viewDividendeMonatentwicklungSummiert:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(DividendeMonatentwicklungSummiertView).Name))
                        Container.NavigationService.Navigate(new DividendeMonatentwicklungSummiertView());
                    break;
                case ViewType.viewSteuerGesamtentwicklungSummiert:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(SteuerGesamtentwicklungSummiertView).Name))
                        Container.NavigationService.Navigate(new SteuerGesamtentwicklungSummiertView());
                    break;
                case ViewType.viewSteuerartGesamtentwicklungSummiert:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(SteuerartGesamtentwicklungSummiertView).Name))
                        Container.NavigationService.Navigate(new SteuerartGesamtentwicklungSummiertView());
                    break;
                case ViewType.viewInvestitionMonatlich:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(InvestitionMonatlichView).Name))
                        Container.NavigationService.Navigate(new InvestitionMonatlichView());
                    break;
                case ViewType.viewInvestitionMonatlichSummiert:
                    if (Container.Content == null || !Container.Content.GetType().Name.Equals(typeof(InvestitionMonatlichSummiertView).Name))
                        Container.NavigationService.Navigate(new InvestitionMonatlichSummiertView());
                    break;
                default:
                    break;

                    
            }
        }


        private void ReceiceOpenStammdatenMessage(BaseStammdatenMessage<StammdatenTypes> m)
        {
            StammdatenView view = null;
            switch (m.Stammdaten)
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
                case StammdatenTypes.vorbelegung:
                    view = new VorbelegungView();
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
            _ = view.ShowDialog();
        }

        private void ReceiceOpenStartingViewMessage()
        {
            _ = new StartingProgrammView().ShowDialog();
        }

        private void ReceiceOpenLoginViewMessage()
        {
            _ = new LoginView()
            {
                Owner = Application.Current.MainWindow
            }.ShowDialog();
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
