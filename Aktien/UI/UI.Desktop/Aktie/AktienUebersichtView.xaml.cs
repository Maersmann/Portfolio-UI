using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.Aktie;
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

namespace Aktien.UI.Desktop
{
    /// <summary>
    /// Interaktionslogik für AktienUebersichtView.xaml
    /// </summary>
    public partial class AktienUebersichtView : UserControl
    {
        public AktienUebersichtView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenAktieStammdatenMessage>(this, m => ReceiveOpenAktieStammdatenMessage( m ));
            Messenger.Default.Register<DeleteAktieErfolgreichMessage>(this, m => ReceiveDeleteAktieErfolgreich());
            Messenger.Default.Register<OpenDividendeUebersichtMessage>(this, m => ReceiveOpenDividendeUebersichtMessage(m));
           
        }

        private void ReceiveOpenDividendeUebersichtMessage(OpenDividendeUebersichtMessage m)
        {
            var view = new DividendenUebersichtView();

            if (view.DataContext is DividendenUebersichtViewModel model)
                model.LoadData(m.AktieID);

            Window window = new Window
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
                 
            };

            window.ShowDialog();
        }

        private void ReceiveOpenAktieStammdatenMessage( OpenAktieStammdatenMessage message )
        {
            var view = new AktieStammdatenView();
            if (view.DataContext is AktieStammdatenViewModel model)
            {
                if (message.State == Data.Types.State.Bearbeiten)
                {
                    model.AktieID = message.AktieID;
                }
                
            }
            bool? Result = view.ShowDialog();

            if (Result.GetValueOrDefault(false) && (this.DataContext is AktienUebersichtViewModel modelUebersicht))
            {
                modelUebersicht.LoadData();
            }
        }
    
        private void ReceiveDeleteAktieErfolgreich()
        {
            MessageBox.Show("Aktie erfolgreich gelöscht.");
        }
    }
}
