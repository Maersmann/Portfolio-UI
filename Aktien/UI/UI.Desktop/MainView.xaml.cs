﻿using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.Aktie;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.Base;
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
using Aktien.UI.Desktop.Aktie;
using Aktien.Data.Types;
using Aktien.UI.Desktop.Depot;

namespace Aktien.UI.Desktop
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainView
    {

        private static AktienUebersichtView aktienUebersichtView;
        private static DepotUebersichtView depotUebersichtView;

        public MainView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenViewMessage>(this, m => ReceiveOpenViewMessage(m));

            Naviagtion(ViewType.viewAktienUebersicht);
        }

        private void ReceiveOpenViewMessage(OpenViewMessage m)
        {
            Naviagtion(m.ViewType);
        }

        public void Naviagtion(ViewType inType)
        {
            switch (inType)
            {
                case ViewType.viewAktienUebersicht:
                    aktienUebersichtView = aktienUebersichtView ?? new AktienUebersichtView();
                    Container.NavigationService.Navigate(aktienUebersichtView);
                    break;
                case ViewType.viewAktieGekauft:
                    new BuyOrderView().ShowDialog();
                    break;
                case ViewType.viewDepotUebersicht:
                    depotUebersichtView = depotUebersichtView ?? new DepotUebersichtView();
                    Container.NavigationService.Navigate(depotUebersichtView);
                    break;
                default:
                    break;
            }
        }

    }

}
