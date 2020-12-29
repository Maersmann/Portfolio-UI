using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.DividendeMessages;
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

namespace UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendenUebersicht.xaml
    /// </summary>
    public partial class DividendenUebersichtView : UserControl
    {
        public DividendenUebersichtView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenDividendeStammdatenNeuMessage>(this, m => ReceiveOpenDividendeStammdatenNeuMessage(m));
        }

        private void ReceiveOpenDividendeStammdatenNeuMessage(OpenDividendeStammdatenNeuMessage m)
        {
            var view = new DividendeStammdatenView();
            if (view.DataContext is DividendeStammdatenViewModel model)
            {
                model.AktieID = m.AktieID;
            }
            bool? Result = view.ShowDialog();

            if ((Result.GetValueOrDefault(false)) && (this.DataContext is DividendenUebersichtViewModel modelUebersicht))
            {
                Messenger.Default.Send<LoadDividendeFuerAktieMessage>(new LoadDividendeFuerAktieMessage { AktieID = m.AktieID });
            }
        }
    }
}
