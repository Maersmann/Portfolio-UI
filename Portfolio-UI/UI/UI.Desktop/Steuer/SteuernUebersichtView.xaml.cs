using Aktien.Data.Types;
using Base.Logic.Types;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.Desktop.Steuer
{
    /// <summary>
    /// Interaktionslogik für SteuernUebersichtView.xaml
    /// </summary>
    public partial class SteuernUebersichtView : Window
    {
        public SteuernUebersichtView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<OpenSteuerStammdatenMessage<StammdatenTypes>, string>(this, "SteuernUebersicht", (r,m) => ReceiveOpenSteuerStammdatenMessage(m));
        }

        private static void ReceiveOpenSteuerStammdatenMessage(OpenSteuerStammdatenMessage<StammdatenTypes> m)
        {
            SteuerStammdatenView view = new()
            { 
                Owner = Application.Current.MainWindow
            };
            if (view.DataContext is SteuerStammdatenViewModel model)
            {
                model.LoadSteuerArts(m.Steuerarts);
                if (m.State.Equals(State.Bearbeiten))
                {
                    model.ZeigeStammdatenAn(m.Steuer);
                }
                _ = view.ShowDialog();
                if(model.Gespeichert)
                {
                    m.Callback(true, model.NewData);
                }
                else
                {
                    m.Callback(false, null);
                }
            }
        }

        public void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Unregister<OpenSteuerStammdatenMessage<StammdatenTypes>, string>(this, "SteuernUebersicht");
            if (DataContext is SteuernUebersichtViewModel model)
            {
                model.ExceuteCallBack();
            }

        }
    }
}
