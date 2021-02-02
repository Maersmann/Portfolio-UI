﻿using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.DividendeViewModels;
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
using System.Windows.Shapes;

namespace Aktien.UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendenUebersichtAuswahl.xaml
    /// </summary>
    public partial class DividendenUebersichtAuswahlView : Window
    {
        public DividendenUebersichtAuswahlView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenDividendeUebersichtMessage>(this, m => ReceiveOpenDividendeUebersichtMessage(m)); 
            Messenger.Default.Register<OpenDividendeErhaltenViewMessage>(this, m => ReceiveOpenDividendeErhaltenViewMessage(m));
        }

        private void ReceiveOpenDividendeErhaltenViewMessage(OpenDividendeErhaltenViewMessage m)
        {
            this.Close();
            var view = new DividendeErhaltenView();

            if (view.DataContext is DividendeErhaltenViewModel model)
                model.AktieID(m.AktieID);

            view.ShowDialog();
        }

        private void ReceiveOpenDividendeUebersichtMessage(OpenDividendeUebersichtMessage m)
        {
            this.Close();
            var view = new DividendenUebersichtView();

            if (view.DataContext is DividendenUebersichtViewModel model)
                model.LoadData(m.AktieID);

            Window window = new Window
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen

            };

            window.ShowDialog();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<OpenDividendeUebersichtMessage>(this);
            Messenger.Default.Unregister<OpenDividendeErhaltenViewMessage>(this);
        }
    }
}
