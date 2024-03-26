using CommunityToolkit.Mvvm.Messaging;
using Aktien.Logic.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aktien.Data.Types;

namespace UI.Desktop.Base
{
    public class StammdatenView : Window
    {
        string registerToken;
        public StammdatenView()
        {
            this.Unloaded += Window_Unloaded;
        }

        public void RegisterStammdatenGespeichertMessage(StammdatenTypes types)
        {
            registerToken = types.ToString();
            WeakReferenceMessenger.Default.Register<StammdatenGespeichertMessage, string>(this, types.ToString(), (r, m) => ReceiveStmmdatenGespeichertMessage(m));
        }

        private void ReceiveStmmdatenGespeichertMessage(StammdatenGespeichertMessage m)
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

        internal void MessageWithToken(string token)
        {
            registerToken = token;
            WeakReferenceMessenger.Default.Register<StammdatenGespeichertMessage, string>(this, token, (r, m) => ReceiveStmmdatenGespeichertMessage(m));
        }

        public virtual void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<StammdatenGespeichertMessage, string>(this, registerToken);
        }
    }
}
