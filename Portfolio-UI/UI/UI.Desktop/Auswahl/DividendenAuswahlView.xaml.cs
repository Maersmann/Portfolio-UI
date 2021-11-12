using Aktien.Data.Types;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.AuswahlViewModels;
using Aktien.Logic.UI.DividendeViewModels;
using Aktien.UI.Desktop.Dividende;
using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Shapes;

namespace Aktien.UI.Desktop.Auswahl
{
    /// <summary>
    /// Interaktionslogik für DividendenAuswahlView.xaml
    /// </summary>
    public partial class DividendenAuswahlView : Window
    {
        public DividendenAuswahlView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenDividendeStammdatenMessage<StammdatenTypes>>(this, m => ReceiveOpenDividendeStammdatenMessage(m));
        }

        private void ReceiveOpenDividendeStammdatenMessage(OpenDividendeStammdatenMessage<StammdatenTypes> m)
        {
            var view = new DividendeStammdatenView()
            {
                Owner = Application.Current.MainWindow
            };

            if (view.DataContext is DividendeStammdatenViewModel model)
            {
                model.WertpapierID = m.WertpapierID;
            }
            bool? Result = view.ShowDialog();

            if ((Result.GetValueOrDefault(false)) && (DataContext is DividendenAuswahlViewModel modelUebersicht))
            {
                modelUebersicht.OhneHinterlegteDividende = true;
                modelUebersicht.LoadData(m.WertpapierID);
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<OpenDividendeStammdatenMessage<StammdatenTypes>>(this);
        }
    }
}
