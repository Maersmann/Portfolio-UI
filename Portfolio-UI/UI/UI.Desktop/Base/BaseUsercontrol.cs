using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.UtilMessages;
using Logic.UI.UtilsViewModels;
using System.Windows;
using System.Windows.Controls;
using UI.Desktop.Utils;

namespace UI.Desktop.Base
{
    public class BaseUsercontrol : UserControl
    {
        string token;
        public BaseUsercontrol()
        {
            Unloaded += Window_Unloaded;
        }

        public void RegisterMessages(string token)
        {
            this.token = token;
            WeakReferenceMessenger.Default.Register<OpenBestaetigungViewMessage, string>(this, token, (r, m) => ReceiveOpenBestaetigungViewMessage(m));
        }

        private void ReceiveOpenBestaetigungViewMessage(OpenBestaetigungViewMessage m)
        {
            var Bestaetigung = new BestaetigungView();
            if (Bestaetigung.DataContext is BestaetigungViewModel model)
            {
                model.Beschreibung = m.Beschreibung;
                Bestaetigung.ShowDialog();
                if (model.Bestaetigt)
                {
                    m.Command();
                }
            }

        }

        protected virtual void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<OpenBestaetigungViewMessage, string>(this, token);
        }
    }
}
