using Aktien.Data.Types;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.DividendeViewModels;
using Base.Logic.Types;
using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.DividendeMessages;
using Logic.UI.DividendeViewModels;
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
using UI.Desktop.Base;
using UI.Desktop.Dividende;

namespace UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendeErhaltenUebersichtView.xaml
    /// </summary>
    public partial class DividendeErhaltenUebersichtView : BaseUsercontrol
    {
        public DividendeErhaltenUebersichtView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<OpenErhaltendeDividendeStammdatenMessage<StammdatenTypes>>(this, (r,m) => ReceiveOpenErhaltendeDividendeStammdatenMessage(m));
            WeakReferenceMessenger.Default.Register<OpenDividendeReitAkualiserungMessage, string>(this, "DividendeErhaltenUebersicht", (r, m) => ReceivOpenDividendeReitAkualiserungMessage(m));
            RegisterMessages("DividendeErhaltenUebersicht");
        }

        private static void ReceivOpenDividendeReitAkualiserungMessage(OpenDividendeReitAkualiserungMessage m)
        {
            DividendeReitAktualisierungView view = new();
            if (view.DataContext is DividendeReitAktualisierungViewModel model)
            {
                model.LoadingDividendeErhalten(m.ID);
            }

            view.Owner = Application.Current.MainWindow;

            _ = view.ShowDialog();
        }

        private void ReceiveOpenErhaltendeDividendeStammdatenMessage(OpenErhaltendeDividendeStammdatenMessage<StammdatenTypes> m)
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

            if (Result.GetValueOrDefault(false) && (this.DataContext is DividendeErhaltenUebersichtViewModel modelUebersicht))
            {
                modelUebersicht.LoadData(m.WertpapierID);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<OpenErhaltendeDividendeStammdatenMessage<StammdatenTypes>>(this);
            WeakReferenceMessenger.Default.Unregister<OpenDividendeReitAkualiserungMessage, string>(this, "DividendeErhaltenUebersicht");
        }
    }
}
