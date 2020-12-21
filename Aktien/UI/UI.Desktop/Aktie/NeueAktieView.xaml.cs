using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
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

namespace UI.Desktop.Aktie
{
    /// <summary>
    /// Interaktionslogik für NeueAktieView.xaml
    /// </summary>
    public partial class NeueAktieView : Window
    {
        public NeueAktieView()
        {
            InitializeComponent();
            Messenger.Default.Register<SaveNeueAktieResultMessage>(this, m => ReceiveSaveNeueAktieResultMessage( m));
        }

        private void ReceiveSaveNeueAktieResultMessage( SaveNeueAktieResultMessage inSaveNeueAktieResultMessage )
        {
            if( inSaveNeueAktieResultMessage.Erfolgreich )
            {
                MessageBox.Show("Aktie gespeichert");
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(inSaveNeueAktieResultMessage.Fehlermessage);
            }

        }
    }
}
