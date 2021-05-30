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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.Desktop.Steuer
{
    /// <summary>
    /// Interaktionslogik für SteuernUebersichtView.xaml
    /// </summary>
    public partial class SteuernUebersichtView : UserControl
    {
        public SteuernUebersichtView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenSteuerStammdatenMessage>(this, "SteuernUebersicht", m => ReceiveOpenSteuerStammdatenMessage(m));
        }

        private void ReceiveOpenSteuerStammdatenMessage(OpenSteuerStammdatenMessage m)
        {
            var view = new SteuerStammdatenView();
            if (view.DataContext is SteuerStammdatenViewModel model)
            {
                model.setGruppeInfos(m.SteuergruppeID, m.Typ, m.IstVerknuepfungGespeichert);
                if (m.State.Equals(State.Bearbeiten))
                    model.ZeigeStammdatenAn(m.ID.Value);
                view.ShowDialog();
            }
        }

        public void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<OpenSteuerStammdatenMessage>(this);
            if (this.DataContext is SteuernUebersichtViewModel model)
            {
                model.ExceuteCallBack();
            }

        }
    }
}
