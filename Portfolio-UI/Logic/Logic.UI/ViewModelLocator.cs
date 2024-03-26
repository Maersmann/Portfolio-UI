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
using CommunityToolkit.Mvvm.Messaging;
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
using Logic.UI.DividendeViewModels;
using Logic.UI.UtilsViewModels;
using Logic.UI.WertpapierViewModels;
using Logic.UI.SparplanViewModels;
using Logic.UI.ZinsenViewModels;
using Logic.UI.AuswertungViewModels.ZinsenViewModels;

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

        }

        public MainViewModel Main => new();
        public AktieStammdatenViewModel AktieStammdaten => new();
        public AktienUebersichtViewModel AktienUebersicht => new();
        public DividendeStammdatenViewModel DividendeStammdaten => new();
        public DividendenUebersichtViewModel DividendenUebersicht => new();
        public BuyOrderViewModel BuyOrder => new ();
        public DepotUebersichtViewModel DepotUebersicht => new();
        public OrderUebersichtViewModel OrderUebersicht => new();
        public AktieUebersichtPageViewModel AktieUebersichtPage => new();
        public DividendeErhaltenViewModel DividendeErhalten => new();
        public DividendenUebersichtAuswahlViewModel DividendenUebersichtAuswahl => new();
        public DividendenAuswahlViewModel DividendenAuswahl => new();
        public DividendeErhaltenUebersichtViewModel DividendeErhaltenUebersicht => new();
        public ETFStammdatenViewModel ETFStammdaten => new();
        public ETFGesamtUebersichtViewModel ETFGesamtUebersicht => new();
        public ETFUebersichtPageViewModel ETFUebersichtPage => new();
        public DepotUebersichtPageViewModel DepotUebersichtPage => new();
        public WertpapierGesamtUebersichtViewModel WertpapierGesamtUebersicht => new();
        public DerivateStammdatenViewModel DerivateStammdaten => new();
        public DerivateGesamtUebersichtViewModel DerivateGesamtUebersicht => new();
        public EinnahmeStammdatenViewModel EinnahmeStammdaten => new();
        public EinnahmenUebersichtViewModel EinnahmenUebersicht =>  new();
        public AusgabeStammdatenViewModel AusgabeStammdaten => new();
        public AusgabenUebersichtViewModel AusgabenUebersicht => new();
        public DividendeProStueckAnpassenViewModel DividendeProStueckAnpassen => new();
        public EinnahmenAusgabenUebersichtViewModel EinnahmenAusgaben => new();
        public DatenAnpassungViewModel DatenAnpassung => new();
        public WertpapierAuswahlViewModel WertpapierAuswahl => new();
        public ReverseSplitEintragenViewModel ReverseSplitEintragen => new();
        public StartingProgrammViewModel StartingProgramm => new();
        public SteuerartenUebersichtViewModel SteuerartenUebersicht => new();
        public SteuerartStammdatenViewModel SteuerartStammdaten => new();
        public SteuernUebersichtViewModel SteuernUebersicht => new ();
        public SteuerStammdatenViewModel SteuerStammdaten => new ();
        public BackendSettingsViewModel BackendSettings => new();
        public KonfigruationViewModel Konfigruation => new();
        public DividendeEntwicklungMonatlichViewModel DividendeEntwicklungMonatlich => new();
        public DividendeVergleichMonatViewModel DividendeVergleichMonat => new();
        public SteuerartMonatAuswertungViewModel SteuerartMonatAuswertung => new();
        public SteuerMonatJahresVergleichAuswertungViewModel SteuerartMonatJahresVergleichAuswertung => new();
        public DividendeWertpapierAuswertungViewModel DividendeWertpapierAuswertung => new();
        public SteuerMonatAuswertungViewModel SteuerMonatAuswertung => new();
        public DividendeWertpapierEntwicklungMonatlichViewModel DividendeWertpapierEntwicklung => new();
        public InfoViewModel Info => new();
        public DividendenErhaltenImMonatViewModel DividendenErhaltenImMonat => new();
        public DividendenErhaltenImJahrViewModel DividendenErhaltenImJahr => new();
        public OrderBuchViewModel OrderBuch => new();
        public LoginViewModel Login => new();
        public SteuerJahresgesamtbetragAuswertungViewModel SteuerJahresgesamtbetragAuswertung => new();
        public SteuerMonatgesamtbetragAuswertungViewModel SteuerMonatgesamtbetragAuswertung => new();
        public DividendeGesamtentwicklungMonatlichSummiertViewModel DividendeGesamtentwicklungSummiert => new();
        public DividendeGesamtentwicklungJaehrlichSummiertViewModel DividendeJahresentwicklungSummiert => new();
        public DividendeMonatentwicklungSummiertViewModel DividendeMonatentwicklungSummiert => new();
        public VorbelegungViewModel Vorbelegung => new();
        public SteuerGesamtentwicklungSummiertViewModel SteuerGesamtentwicklungSummiert => new();
        public SteuerartGesamtentwicklungSummiertViewModel SteuerartGesamtentwicklungSummiert => new();
        public InvestitionMonatlichViewModel InvestitionMonatlich => new();
        public InvestitionMonatlichSummiertViewModel InvestitionMonatlichSummiert => new();
        public DividendeReitAktualisierungViewModel DividendeReitAktualisierung => new();
        public BestaetigungViewModel Bestaetigung => new();
        public DividendeMonatlichJahresentwicklungViewModel DividendeMonatlichJahresentwicklung => new();
        public SplitEintragenViewModel AktienSplitEintragen => new();
        public SparplanUebersichtViewModel SparplanUebersicht => new();
        public SparplanStammdatenViewModel SparplanStammdaten => new();
        public SparplanAusfuehrenUebersichtViewModel SparplanAusfuehrenUebersicht => new();
        public SparplanAusfuehrenViewModel SparplanAusfuehren => new();
        public SparplanHistoryUebersichtViewModel SparplanHistoryUebersicht => new();
        public ErhalteneDividendeEintragenViewModel ErhalteneDividendeEintragen => new();
        public ZinsenErhaltenUebersichtViewModel ZinsenErhaltenUebersicht => new();
        public ZinsenEintragenViewModel ZinsenEintragen => new();
        public ZinsenEntwicklungMonatlichViewModel zinsenEntwicklungMonatlich => new();
        public ZinsenMonatlichJahresentwicklungViewModel ZinsenMonatlichJahresentwicklung => new();
        public ZinsenVergleichMonatViewModel ZinsenVergleichMonat => new();
        public ZinsenGesamtentwicklungMonatlichSummiertViewModel ZinsenGesamtentwicklungMonatlichSummiert => new();
        public ZinsenGesamtentwicklungJaehrlichSummiertViewModel ZinsenGesamtentwicklungJaehrlichSummiert => new();
        public static void Cleanup()
        {

        }
    }
}