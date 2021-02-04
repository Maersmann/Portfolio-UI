using Aktien.Logic.Messages.ETFMessages;
using Aktien.Logic.UI.ETFViewModels;
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

namespace Aktien.UI.Desktop.ETF
{
    /// <summary>
    /// Interaktionslogik für ETFGesamtUebersicht.xaml
    /// </summary>
    public partial class ETFGesamtUebersicht : UserControl
    {
        public ETFGesamtUebersicht()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenETFStammdatenMessage>(this, m => ReceiveOpenETFStammdatenMessage(m));
            Messenger.Default.Register<DeleteEtfErfolgreichMessage>(this, m => ReceiveDeleteEtfErfolgreichMessage());
        }

        private void ReceiveOpenETFStammdatenMessage(OpenETFStammdatenMessage m)
        {
            var view = new ETFStammdatenView();
            if (view.DataContext is ETFStammdatenViewModel model)
            {
                if (m.State == Data.Types.State.Bearbeiten)
                {
                    model.Bearbeiten(m.WertpapierID);
                }

            }
            bool? Result = view.ShowDialog();

            if (Result.GetValueOrDefault(false) && (this.DataContext is ETFGesamtUebersichtViewModel modelUebersicht))
            {
                modelUebersicht.LoadData();
            }
        }

        private void ReceiveDeleteEtfErfolgreichMessage()
        {
            MessageBox.Show("ETF gelöscht.");
        }
    }
}
