using Aktien.Logic.Messages.DerivateMessages;
using Aktien.Logic.UI.DerivateViewModels;
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

namespace Aktien.UI.Desktop.Derivate
{
    /// <summary>
    /// Interaktionslogik für DerivateGesamtUebersichtView.xaml
    /// </summary>
    public partial class DerivateGesamtUebersichtView : UserControl
    {
        public DerivateGesamtUebersichtView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenDerivateStammdatenMessage>(this, m => ReceiveOpenDerivateStammdatenMessage(m));
            Messenger.Default.Register<DeleteDerivateErfolgreichMessage>(this, m => ReceiveDeleteDerivateErfolgreichMessage());
        }

        public string MessageToken
        {
            set
            {
                if (this.DataContext is DerivateGesamtUebersichtViewModel modelUebersicht)
                {
                    modelUebersicht.MessageToken = value;
                }
            }
        }

        private void ReceiveDeleteDerivateErfolgreichMessage()
        {
            MessageBox.Show("Derivate gelöscht.");
        }

        private void ReceiveOpenDerivateStammdatenMessage(OpenDerivateStammdatenMessage m)
        {
            var view = new DerivateStammdatenView();
            if (view.DataContext is DerivateStammdatenViewModel model)
            {
                if (m.State == Data.Types.State.Bearbeiten)
                {
                    model.Bearbeiten(m.WertpapierID);
                }

            }
            view.ShowDialog();
        }
    }
}