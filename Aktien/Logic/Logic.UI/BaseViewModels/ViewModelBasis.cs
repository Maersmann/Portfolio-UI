﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelBasis : ViewModelBase
    {

        public ViewModelBasis()
        {
            messageToken = "";
            CloseCommand = new RelayCommand(() => ExecuteCloseCommand());
            this.CloseWindowCommand = new RelayCommand<Window>(this.ExecuteCloseWindowCommand);
        }

        protected string messageToken;
        public virtual string MessageToken { set { messageToken = value; } }
        public string Title { get; protected set; }
     
        public ICommand CloseCommand { get; private set; }
        public RelayCommand<Window> CloseWindowCommand { get; private set; }

        protected virtual void ExecuteCloseCommand()
        {
            Cleanup();
        }
        protected void ExecuteCloseWindowCommand(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public void SendExceptionMessage(string exceptionMessage)
        {
            Messenger.Default.Send<ExceptionMessage>(new ExceptionMessage { Message = exceptionMessage });
        }
        public void SendInformationMessage(string informationMessage)
        {
            Messenger.Default.Send<InformationMessage>(new InformationMessage { Message = informationMessage });
        }


        public void OnlyNumbersCommand(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void OnlyBetragCommand(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Equals(","))
            {
                e.Handled = true;
            }
            else
            { 
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
        }





    }
}
