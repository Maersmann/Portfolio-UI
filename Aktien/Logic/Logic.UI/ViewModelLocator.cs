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
using Logic.Messages.Aktie;
using Logic.UI.AktieViewModels;
using Logic.UI.DividendeModels;

namespace Logic.UI
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
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public AktieStammdatenViewModel AktieStammdaten => ServiceLocator.Current.GetInstance<AktieStammdatenViewModel>();
        public AktienUebersichtViewModel AktienUebersicht => ServiceLocator.Current.GetInstance<AktienUebersichtViewModel>();
        public DividendeStammdatenViewModel DividendeStammdaten => ServiceLocator.Current.GetInstance<DividendeStammdatenViewModel>();

        public static void Cleanup()
        {

        }

        public static void CleanUpAktieStammdatenView()
        {
            SimpleIoc.Default.Unregister<AktieStammdatenViewModel>();
            SimpleIoc.Default.Register<AktieStammdatenViewModel>();
        }

        public static void CleanUpDividendeStammdatenView()
        {
            SimpleIoc.Default.Unregister<DividendeStammdatenViewModel>();
            SimpleIoc.Default.Register<DividendeStammdatenViewModel>();
        }
    }
}