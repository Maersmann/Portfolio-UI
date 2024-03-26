using Aktien.Data.Types;
using Aktien.Logic.Messages.Base;
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
using UI.Desktop.Base;
using UI.Desktop.Steuer;

namespace UI.Desktop.Zinsen
{
    /// <summary>
    /// Interaktionslogik für ZinsenEintragenView.xaml
    /// </summary>
    public partial class ZinsenEintragenView : StammdatenView
    {
        public ZinsenEintragenView()
        {
            InitializeComponent();
            RegisterStammdatenGespeichertMessage(StammdatenTypes.zinsenErhalten);
            WeakReferenceMessenger.Default.Register<OpenSteuernUebersichtMessage, string>(this, "ZinsenEintragen", (r,m) => ReceiveOpenSteuernUebersichtMessage(m));
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

        public override void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            base.Window_Unloaded(sender, e);
            WeakReferenceMessenger.Default.Unregister<OpenSteuernUebersichtMessage, string>(this, "ZinsenEintragen");
        }
    }
}
