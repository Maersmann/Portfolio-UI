/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:UI.Desktop"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.UI.AktieViewModels;
using Aktien.Logic.UI.DepotViewModels;
using Aktien.Logic.UI.DividendeViewModels;
using Aktien.Logic.UI.AktieViewModels.Page;
using Aktien.Logic.UI.AuswahlViewModels;
using Aktien.Logic.UI.ETFViewModels;
using Aktien.Logic.UI.WertpapierViewModels;
using Aktien.Logic.UI.ETFViewModels.Page;
using Aktien.Logic.UI.DepotViewModels.Page;
using Aktien.Logic.UI.DerivateViewModels;
using Aktien.Logic.UI.OptionenViewModels;
using Logic.UI.SteuerViewModels;
using Logic.UI.KonfigurationViewModels;
using Logic.UI.AuswertungViewModels;
using Logic.UI.OptionenViewModels;
using Logic.UI.AuswertungViewModels.DividendeErhaltenViewModels;
using Logic.UI.DepotViewModels;
using Logic.UI;
using Logic.UI.AuswertungViewModels.SteuerViewModels;
using Logic.UI.UserViewModels;
using Logic.UI.AuswertungViewModels.InvestitionViewModels;

namespace Aktien.Logic.UI
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models                
            }
            else
            {
                // Create run time view services and models                
            }
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AktieStammdatenViewModel>();
            SimpleIoc.Default.Register<AktienUebersichtViewModel>();
            SimpleIoc.Default.Register<DividendeStammdatenViewModel>();
            SimpleIoc.Default.Register<DividendenUebersichtViewModel>();
            SimpleIoc.Default.Register<BuyOrderViewModel>();
            SimpleIoc.Default.Register<DepotUebersichtViewModel>();
            SimpleIoc.Default.Register<OrderUebersichtViewModel>();
            SimpleIoc.Default.Register<AktieUebersichtPageViewModel>();
            SimpleIoc.Default.Register<DividendeErhaltenViewModel>();
            SimpleIoc.Default.Register<DividendenUebersichtAuswahlViewModel>();
            SimpleIoc.Default.Register<DividendenAuswahlViewModel>();
            SimpleIoc.Default.Register<DividendeErhaltenUebersichtViewModel>();
            SimpleIoc.Default.Register<ETFStammdatenViewModel>();
            SimpleIoc.Default.Register<ETFGesamtUebersichtViewModel>(); 
            SimpleIoc.Default.Register<ETFUebersichtPageViewModel>();
            SimpleIoc.Default.Register<DepotUebersichtPageViewModel>();
            SimpleIoc.Default.Register<WertpapierGesamtUebersichtViewModel>();
            SimpleIoc.Default.Register<DerivateGesamtUebersichtViewModel>();
            SimpleIoc.Default.Register<DerivateStammdatenViewModel>();
            SimpleIoc.Default.Register<EinnahmeStammdatenViewModel>();
            SimpleIoc.Default.Register<EinnahmenUebersichtViewModel>();
            SimpleIoc.Default.Register<AusgabeStammdatenViewModel>();
            SimpleIoc.Default.Register<AusgabenUebersichtViewModel>();
            SimpleIoc.Default.Register<EinnahmenAusgabenUebersichtViewModel>();
            SimpleIoc.Default.Register<DatenAnpassungViewModel>();
            SimpleIoc.Default.Register<SteuerartenUebersichtViewModel>();
            SimpleIoc.Default.Register<BackendSettingsViewModel>();
            SimpleIoc.Default.Register<SteuerJahresgesamtbetragAuswertungViewModel>();
            SimpleIoc.Default.Register<SteuerMonatgesamtbetragAuswertungViewModel>();
            SimpleIoc.Default.Register<DividendeGesamtentwicklungSummiertViewModel>();
            SimpleIoc.Default.Register<DividendeJahresentwicklungSummiertViewModel>();
            SimpleIoc.Default.Register<VorbelegungViewModel>();
            SimpleIoc.Default.Register<SteuerGesamtentwicklungSummiertViewModel>();
            SimpleIoc.Default.Register<SteuerartGesamtentwicklungSummiertViewModel>();
            SimpleIoc.Default.Register<InvestitionMonatlichViewModel>();
            SimpleIoc.Default.Register<InvestitionMonatlichSummiertViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public AktieStammdatenViewModel AktieStammdaten => ServiceLocator.Current.GetInstance<AktieStammdatenViewModel>();
        public AktienUebersichtViewModel AktienUebersicht => ServiceLocator.Current.GetInstance<AktienUebersichtViewModel>();
        public DividendeStammdatenViewModel DividendeStammdaten => ServiceLocator.Current.GetInstance<DividendeStammdatenViewModel>();
        public DividendenUebersichtViewModel DividendenUebersicht => ServiceLocator.Current.GetInstance<DividendenUebersichtViewModel>();
        public BuyOrderViewModel BuyOrder => new BuyOrderViewModel();
        public DepotUebersichtViewModel DepotUebersicht => ServiceLocator.Current.GetInstance<DepotUebersichtViewModel>();
        public OrderUebersichtViewModel OrderUebersicht => new OrderUebersichtViewModel();
        public AktieUebersichtPageViewModel AktieUebersichtPage => ServiceLocator.Current.GetInstance<AktieUebersichtPageViewModel>();
        public DividendeErhaltenViewModel DividendeErhalten => new DividendeErhaltenViewModel();
        public DividendenUebersichtAuswahlViewModel DividendenUebersichtAuswahl => ServiceLocator.Current.GetInstance<DividendenUebersichtAuswahlViewModel>();
        public DividendenAuswahlViewModel DividendenAuswahl => ServiceLocator.Current.GetInstance<DividendenAuswahlViewModel>();
        public DividendeErhaltenUebersichtViewModel DividendeErhaltenUebersicht => ServiceLocator.Current.GetInstance<DividendeErhaltenUebersichtViewModel>();
        public ETFStammdatenViewModel ETFStammdaten => ServiceLocator.Current.GetInstance<ETFStammdatenViewModel>();
        public ETFGesamtUebersichtViewModel ETFGesamtUebersicht => ServiceLocator.Current.GetInstance<ETFGesamtUebersichtViewModel>(); 
        public ETFUebersichtPageViewModel ETFUebersichtPage => ServiceLocator.Current.GetInstance<ETFUebersichtPageViewModel>();
        public DepotUebersichtPageViewModel DepotUebersichtPage => ServiceLocator.Current.GetInstance<DepotUebersichtPageViewModel>();
        public WertpapierGesamtUebersichtViewModel WertpapierGesamtUebersicht => ServiceLocator.Current.GetInstance<WertpapierGesamtUebersichtViewModel>();
        public DerivateStammdatenViewModel DerivateStammdaten => ServiceLocator.Current.GetInstance<DerivateStammdatenViewModel>();
        public DerivateGesamtUebersichtViewModel DerivateGesamtUebersicht => ServiceLocator.Current.GetInstance<DerivateGesamtUebersichtViewModel>();
        public EinnahmeStammdatenViewModel EinnahmeStammdaten => ServiceLocator.Current.GetInstance<EinnahmeStammdatenViewModel>();
        public EinnahmenUebersichtViewModel EinnahmenUebersicht => ServiceLocator.Current.GetInstance<EinnahmenUebersichtViewModel>();
        public AusgabeStammdatenViewModel AusgabeStammdaten => ServiceLocator.Current.GetInstance<AusgabeStammdatenViewModel>();
        public AusgabenUebersichtViewModel AusgabenUebersicht => ServiceLocator.Current.GetInstance<AusgabenUebersichtViewModel>();
        public DividendeProStueckAnpassenViewModel DividendeProStueckAnpassen => new DividendeProStueckAnpassenViewModel();
        public EinnahmenAusgabenUebersichtViewModel EinnahmenAusgaben => ServiceLocator.Current.GetInstance<EinnahmenAusgabenUebersichtViewModel>();
        public DatenAnpassungViewModel DatenAnpassung => new DatenAnpassungViewModel();
        public WertpapierAuswahlViewModel WertpapierAuswahl => new WertpapierAuswahlViewModel();
        public ReverseSplitEintragenViewModel ReverseSplitEintragen => new ReverseSplitEintragenViewModel();
        public StartingProgrammViewModel StartingProgramm => new StartingProgrammViewModel();
        public SteuerartenUebersichtViewModel SteuerartenUebersicht => ServiceLocator.Current.GetInstance<SteuerartenUebersichtViewModel>();
        public SteuerartStammdatenViewModel SteuerartStammdaten => new SteuerartStammdatenViewModel();
        public SteuernUebersichtViewModel SteuernUebersicht => new SteuernUebersichtViewModel();
        public SteuerStammdatenViewModel SteuerStammdaten => new SteuerStammdatenViewModel();
        public BackendSettingsViewModel BackendSettings => ServiceLocator.Current.GetInstance<BackendSettingsViewModel>();
        public KonfigruationViewModel Konfigruation => new KonfigruationViewModel();
        public DividendeEntwicklungMonatlichViewModel DividendeEntwicklungMonatlich => new DividendeEntwicklungMonatlichViewModel();
        public DividendeVergleichMonatViewModel DividendeVergleichMonat => new DividendeVergleichMonatViewModel();
        public SteuerartMonatAuswertungViewModel SteuerartMonatAuswertung => new SteuerartMonatAuswertungViewModel();
        public SteuerMonatJahresVergleichAuswertungViewModel SteuerartMonatJahresVergleichAuswertung => new SteuerMonatJahresVergleichAuswertungViewModel();
        public DividendeWertpapierAuswertungViewModel DividendeWertpapierAuswertung => new DividendeWertpapierAuswertungViewModel();
        public SteuerMonatAuswertungViewModel SteuerMonatAuswertung => new SteuerMonatAuswertungViewModel();
        public DividendeWertpapierEntwicklungMonatlichViewModel DividendeWertpapierEntwicklung => new DividendeWertpapierEntwicklungMonatlichViewModel();
        public InfoViewModel Info => new InfoViewModel();
        public DividendenErhaltenImMonatViewModel DividendenErhaltenImMonat => new DividendenErhaltenImMonatViewModel();
        public DividendenErhaltenImJahrViewModel DividendenErhaltenImJahr => new DividendenErhaltenImJahrViewModel();
        public OrderBuchViewModel OrderBuch => new OrderBuchViewModel();
        public LoginViewModel Login => new LoginViewModel();
        public SteuerJahresgesamtbetragAuswertungViewModel SteuerJahresgesamtbetragAuswertung => new SteuerJahresgesamtbetragAuswertungViewModel();
        public SteuerMonatgesamtbetragAuswertungViewModel SteuerMonatgesamtbetragAuswertung => new SteuerMonatgesamtbetragAuswertungViewModel();
        public DividendeGesamtentwicklungSummiertViewModel DividendeGesamtentwicklungSummiert => new DividendeGesamtentwicklungSummiertViewModel();
        public DividendeJahresentwicklungSummiertViewModel DividendeJahresentwicklungSummiert => new DividendeJahresentwicklungSummiertViewModel();
        public DividendeMonatentwicklungSummiertViewModel DividendeMonatentwicklungSummiert => new DividendeMonatentwicklungSummiertViewModel();
        public VorbelegungViewModel Vorbelegung => new VorbelegungViewModel();
        public SteuerGesamtentwicklungSummiertViewModel SteuerGesamtentwicklungSummiert => new SteuerGesamtentwicklungSummiertViewModel();
        public SteuerartGesamtentwicklungSummiertViewModel SteuerartGesamtentwicklungSummiert => new SteuerartGesamtentwicklungSummiertViewModel();
        public InvestitionMonatlichViewModel InvestitionMonatlich => new InvestitionMonatlichViewModel();
        public InvestitionMonatlichSummiertViewModel InvestitionMonatlichSummiert => new InvestitionMonatlichSummiertViewModel();
        public static void Cleanup()
        {

        }
    }
}