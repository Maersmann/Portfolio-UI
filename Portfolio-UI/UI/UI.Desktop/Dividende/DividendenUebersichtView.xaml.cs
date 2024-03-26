using Aktien.Data.Types;
using CommunityToolkit.Mvvm.Messaging;
using Aktien.Logic.Messages.DividendeMessages;
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
using Base.Logic.Types;
using UI.Desktop.Dividende;
using UI.Desktop.Base;

namespace UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendenUebersicht.xaml
    /// </summary>
    public partial class DividendenUebersichtView : BaseUsercontrol
    {
        public DividendenUebersichtView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<OpenDividendeStammdatenMessage<StammdatenTypes>>(this, (r, m) => ReceiveOpenDividendeStammdatenMessage(m));
            RegisterMessages("DividendeUebersicht");
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
                if (m.State == State.Bearbeiten)
                {
                    model.Bearbeiten( m.DividendeID.GetValueOrDefault() );
                }
            }
            view.ShowDialog();
        }

        private void DataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<OpenDividendeStammdatenMessage<StammdatenTypes>>(this);
        }
    }
}
