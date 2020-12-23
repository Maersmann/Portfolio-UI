using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
using Logic.Messages.AktieMessages;
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
using Data.Types;

namespace UI.Desktop
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainView
    {
        private bool canAktualisieren = false;

        private static AktienUebersichtView aktienUebersichtView;

        public MainView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenAktieStammdatenMessage>(this, m => ReceiveOpenNeueAktieViewMessage());
            Messenger.Default.Register<OpenViewMessage>(this, m => ReceiveOpenViewMessage(m));

            Naviagtion(ViewType.viewAktienUebersicht);
        }

        private void ReceiveOpenViewMessage(OpenViewMessage m)
        {
            Naviagtion(m.ViewType);
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
            
            bool? Result = new AktieStammdatenView().ShowDialog();

            if (Result.GetValueOrDefault(false))
            {
                canAktualisieren = true;
                Container.NavigationService.Refresh();
            }
        }

        public void Naviagtion(ViewType inType)
        {
            switch (inType)
            {
                case ViewType.viewAktienUebersicht:
                    aktienUebersichtView = aktienUebersichtView ?? new AktienUebersichtView();
                    Container.NavigationService.Navigate(aktienUebersichtView);
                    break;
                default:
                    break;
            }
        }

    }

}
