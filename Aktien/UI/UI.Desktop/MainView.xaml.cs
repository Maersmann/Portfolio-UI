using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
using Logic.Messages.Base;
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
using UI.Desktop.Aktie;

namespace UI.Desktop
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainView
    {
        private bool canAktualisieren = false;
        public MainView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenNeueAktieMessage>(this, m => ReceiveOpenNeueAktieViewMessage());
            Messenger.Default.Register<OpenAuswahlAktieMessage>(this, m => ReceiveOpenAuswahlAktieMessage());

            Container.NavigationService.Navigate( new AktienUebersichtView() );
        }

        private void Container_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
             if (e.Content == Container.NavigationService.Content)
             {
                if (canAktualisieren)
                    Messenger.Default.Send(new AktualisiereViewMessage { });
            }
            canAktualisieren = false;
        }

        private void ReceiveOpenNeueAktieViewMessage()
        {
            
            bool? Result = new NeueAktieView().ShowDialog();

            if (Result.GetValueOrDefault(false))
            {
                canAktualisieren = true;
                Container.NavigationService.Refresh();
            }
        }

        private void ReceiveOpenAuswahlAktieMessage()
        {
           Container.NavigationService.Navigate(new AktienUebersichtView());
        }

    }

}
