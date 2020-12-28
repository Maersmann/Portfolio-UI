using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
using Logic.Messages.Base;
using Logic.Messages.DividendeMessages;
using Logic.UI.AktieViewModels;
using Logic.UI.DividendeModels;
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
using UI.Desktop.Dividende;

namespace UI.Desktop
{
    /// <summary>
    /// Interaktionslogik für AktienUebersichtView.xaml
    /// </summary>
    public partial class AktienUebersichtView : Page
    {
        public AktienUebersichtView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenAktieStammdatenBearbeitenMessage>(this, m => ReceiveOpenAktieStammdatenBearbeitenMessage( m ));
            Messenger.Default.Register<DeleteAktieErfolgreichMessage>(this, m => ReceiveDeleteAktieErfolgreich());
            Messenger.Default.Register<OpenDividendeStammdatenNeuMessage>(this, m => ReceiveOpenDividendeStammdatenNeuMessage(m));
        }

        private void ReceiveOpenAktieStammdatenBearbeitenMessage( OpenAktieStammdatenBearbeitenMessage message )
        {
            var view = new AktieStammdatenView();
            if (view.DataContext is AktieStammdatenViewModel model)
            {
                model.AktieID = message.AktieID;
            }
            bool? Result = view.ShowDialog();

            if (Result.GetValueOrDefault(false))
            {
                Messenger.Default.Send(new AktualisiereViewMessage { });
            }
        }
    
        private void ReceiveDeleteAktieErfolgreich()
        {
            MessageBox.Show("Aktie erfolgreich gelöscht.");
            Messenger.Default.Send(new AktualisiereViewMessage { });
        }

        private void ReceiveOpenDividendeStammdatenNeuMessage( OpenDividendeStammdatenNeuMessage message )
        {
            var view = new DividendeStammdatenView();
            if ( view.DataContext is DividendeStammdatenViewModel model )
            {
                model.Aktienname = message.Aktienname;
                model.AktienID = message.AktienID;
            }
            view.ShowDialog();
        }
    }
}
