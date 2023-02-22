using Aktien.Data.Types;
using GalaSoft.MvvmLight.Messaging;
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
            Messenger.Default.Register<OpenSteuernUebersichtMessage>(this, "ZinsenEintragen", m => ReceiveOpenSteuernUebersichtMessage(m));
        }

        private void ReceiveOpenSteuernUebersichtMessage(OpenSteuernUebersichtMessage m)
        {
            var view = new SteuernUebersichtView();

            if (view.DataContext is SteuernUebersichtViewModel model)
            {
                model.SetCallback(m.Callback);
                model.setSteuern(m.Steuern);
            }
            Window window = new Window
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();

        }
    }
}
