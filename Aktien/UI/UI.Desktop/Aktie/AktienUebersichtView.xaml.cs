using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
using Logic.Messages.Base;
using Logic.UI.AktieUI;
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
    /// Interaktionslogik für AktienUebersichtView.xaml
    /// </summary>
    public partial class AktienUebersichtView : Page
    {
        public AktienUebersichtView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenAktieStammdatenBearbeitenMessage>(this, m => ReceiveOpenAktieStammdatenBearbeitenMessage( m ));
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
    }
}
