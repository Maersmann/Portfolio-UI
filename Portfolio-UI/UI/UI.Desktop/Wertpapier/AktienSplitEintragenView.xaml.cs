﻿using Aktien.Logic.Messages.Base;
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

namespace UI.Desktop.Wertpapier
{
    /// <summary>
    /// Interaktionslogik für AktienSplitEintragenView.xaml
    /// </summary>
    public partial class AktienSplitEintragenView : Window
    {
        public AktienSplitEintragenView()
        {
            InitializeComponent();
            Messenger.Default.Register<CloseViewMessage>(this, "AktienSplitEintragen", m => ReceivCloseViewMessage());
        }
        private void ReceivCloseViewMessage()
        {
            GetWindow(this).Close();
        }
    }
}
