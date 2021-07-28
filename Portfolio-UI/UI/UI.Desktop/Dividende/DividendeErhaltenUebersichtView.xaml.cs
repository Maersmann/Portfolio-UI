using Aktien.Data.Types;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.DividendeViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aktien.UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendeErhaltenUebersichtView.xaml
    /// </summary>
    public partial class DividendeErhaltenUebersichtView : UserControl
    {
        public DividendeErhaltenUebersichtView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenErhaltendeDividendeStammdatenMessage>(this, m => ReceiveOpenErhaltendeDividendeStammdatenMessage(m));
        }

        private void ReceiveOpenErhaltendeDividendeStammdatenMessage(OpenErhaltendeDividendeStammdatenMessage m)
        {
            var view = new DividendeErhaltenView()
            {
                Owner = Application.Current.MainWindow
            };
            if (view.DataContext is DividendeErhaltenViewModel model)
            {
                model.WertpapierID( m.WertpapierID );
                if (m.State == State.Bearbeiten)
                {
                    model.Bearbeiten(m.ID.GetValueOrDefault());
                }
            }
            bool? Result = view.ShowDialog();

            if ((Result.GetValueOrDefault(false)) && (this.DataContext is DividendeErhaltenUebersichtViewModel modelUebersicht))
            {
                modelUebersicht.LoadData(m.WertpapierID);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<OpenErhaltendeDividendeStammdatenMessage>(this);
        }
    }
}
