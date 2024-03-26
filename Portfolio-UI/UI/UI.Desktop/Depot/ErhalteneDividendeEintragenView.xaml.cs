using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DividendeMessages;
using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.SteuernMessages;
using Logic.UI.SteuerViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI.Desktop.Steuer;

namespace UI.Desktop.Depot
{
    /// <summary>
    /// Interaktionslogik für ErhalteneDividendeEintragenView.xaml
    /// </summary>
    public partial class ErhalteneDividendeEintragenView : Window
    {
        public ErhalteneDividendeEintragenView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<CloseViewMessage, string>(this, "ErhalteneDividendeEintragen", (r,m) => ReceivCloseViewMessage());
            WeakReferenceMessenger.Default.Register<OpenSteuernUebersichtMessage, string>(this, "ErhalteneDividendeEintragen", (r, m) => ReceiveOpenSteuernUebersichtMessage(m));
        }
        private void ReceiveOpenSteuernUebersichtMessage(OpenSteuernUebersichtMessage m)
        {
            var view = new SteuernUebersichtView();

            if (view.DataContext is SteuernUebersichtViewModel model)
            {
                model.SetCallback(m.Callback);
                model.setSteuern(m.Steuern);
            }
            view.Owner = Application.Current.MainWindow;
            view.ShowDialog();
        }

        private void ReceivCloseViewMessage()
        {
            GetWindow(this).Close();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<OpenSteuernUebersichtMessage, string>(this, "ErhalteneDividendeEintragen");
            WeakReferenceMessenger.Default.Unregister<CloseViewMessage, string>(this, "ErhalteneDividendeEintragen");

        }
    }
}