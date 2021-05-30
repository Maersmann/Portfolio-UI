using GalaSoft.MvvmLight;
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
using System.Net.Http;
using System.Net.Http.Headers;
using Aktien.Logic.Core;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelBasis : ViewModelBase
    {

        public ViewModelBasis()
        {
            messageToken = "";
            CloseCommand = new RelayCommand(() => ExecuteCloseCommand());
            this.CloseWindowCommand = new RelayCommand<Window>(this.ExecuteCloseWindowCommand);
            SetConnection();
        }

        protected HttpClient Client { get; set; }

        protected string messageToken;
        public virtual string MessageToken { set { messageToken = value; } }
        public string Title { get; protected set; }
     
        public ICommand CloseCommand { get; private set; }
        public RelayCommand<Window> CloseWindowCommand { get; private set; }

        protected virtual void ExecuteCloseCommand()
        {
            Cleanup();
        }
        protected virtual void ExecuteCloseWindowCommand( Window window)
        {              
            if (window != null)
            {
                window.Close();
            }          
        }

        public static void SendExceptionMessage(string exceptionMessage)
        {
            Messenger.Default.Send<ExceptionMessage>(new ExceptionMessage { Message = exceptionMessage });
        }
        public static void SendInformationMessage(string informationMessage)
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

        private void SetConnection()
        {
            string url;
            if (GlobalVariables.BackendServer_IP == null || GlobalVariables.BackendServer_IP.Equals(""))
                url = "https://localhost:5001";
            else
                url = GlobalVariables.BackendServer_URL;
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            Client = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri(url + "/")
            };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
        }




    }
}
