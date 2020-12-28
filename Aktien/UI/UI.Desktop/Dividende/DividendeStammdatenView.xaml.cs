using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.DividendeMessages;
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

namespace UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendeStammdatenView.xaml
    /// </summary>
    public partial class DividendeStammdatenView : Window
    {
        public DividendeStammdatenView()
        {
            InitializeComponent();
            Messenger.Default.Register<NeueDividendeGespeichertMessage>(this, m => ReceiveNeueDividendeGespeichertMessage(m));
        }

        private void ReceiveNeueDividendeGespeichertMessage(NeueDividendeGespeichertMessage m)
        {
            if (m.Erfolgreich)
            {
                MessageBox.Show(m.Message);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show(m.Message);
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<NeueDividendeGespeichertMessage>(this);
        }
    }
}
