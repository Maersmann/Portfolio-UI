using Aktien.Logic.Messages.Base;
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

namespace UI.Desktop
{
    /// <summary>
    /// Interaktionslogik für LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            Messenger.Default.Register<CloseViewMessage>(this, "Login", m => ReceivCloseViewMessage());
        }

        private void ReceivCloseViewMessage()
        {
            Window.GetWindow(this).Close();
        }
    }
}
