using Aktien.Data.Types;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.UI.AuswahlViewModels;
using Aktien.UI.Desktop.Auswahl;
using GalaSoft.MvvmLight.Messaging;
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

namespace UI.Desktop.Sparplan
{
    /// <summary>
    /// Interaktionslogik für SparplanStammdatenView.xaml
    /// </summary>
    public partial class SparplanStammdatenView : StammdatenView
    {
        public SparplanStammdatenView()
        {
            InitializeComponent();
            RegisterStammdatenGespeichertMessage(StammdatenTypes.sparplan);
            Messenger.Default.Register<OpenWertpapierAuswahlMessage>(this, "SparplanStammdaten", m => ReceiveOpenWertpapierAuswahlMessage(m));
        }

        private void ReceiveOpenWertpapierAuswahlMessage(OpenWertpapierAuswahlMessage m)
        {
            var view = new WertpapierAuswahlView()
            {
                Owner = Application.Current.MainWindow
            };

            if (view.DataContext is WertpapierAuswahlViewModel model)
            {
                model.SetTyp(m.WertpapierTypes);
                _ = view.ShowDialog();
                if (model.AuswahlGetaetigt && model.ID().HasValue)
                {
                    m.Callback(true, model.ID().Value);
                }
                else
                {
                    m.Callback(false, 0);
                }
            }
        }

        public override void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            base.Window_Unloaded(sender, e);
            Messenger.Default.Unregister<OpenWertpapierAuswahlMessage>(this);
        }
    }
}