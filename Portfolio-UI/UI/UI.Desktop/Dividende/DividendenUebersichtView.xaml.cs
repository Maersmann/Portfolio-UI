﻿using Aktien.Data.Types;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.DividendeMessages;
using Aktien.Logic.UI.DividendeViewModels;
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

namespace Aktien.UI.Desktop.Dividende
{
    /// <summary>
    /// Interaktionslogik für DividendenUebersicht.xaml
    /// </summary>
    public partial class DividendenUebersichtView : UserControl
    {
        public DividendenUebersichtView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenDividendeStammdatenMessage>(this, m => ReceiveOpenDividendeStammdatenMessage(m));
        }


        private void ReceiveOpenDividendeStammdatenMessage(OpenDividendeStammdatenMessage m)
        {
            var view = new DividendeStammdatenView();
            if (view.DataContext is DividendeStammdatenViewModel model)
            {
                model.WertpapierID = m.WertpapierID;
                if (m.State == State.Bearbeiten)
                {
                    model.Bearbeiten( m.DividendeID.GetValueOrDefault() );
                }
            }
            view.ShowDialog();
        }

        private void DataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<OpenDividendeStammdatenMessage>(this);
        }
    }
}
