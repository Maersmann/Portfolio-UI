using Aktien.Logic.Core;
using Aktien.Logic.Messages.Base;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Logic.UI.Helper
{
    public class BackendHelper
    {
        public void CheckServerIsOnline(string ip, int port)
        {
            using TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(ip, port);
                GlobalVariables.ServerIsOnline = true;
            }
            catch (Exception e)
            {
                GlobalVariables.ServerIsOnline = false;
                Messenger.Default.Send<ExceptionMessage>(new ExceptionMessage { Message = "Backend ist nicht erreichbar" + Environment.NewLine + e.Message });
            }
        }
        public void CheckServerIsOnline(string ip) { CheckServerIsOnline(ip, 80); }
        public void CheckServerIsOnline() 
        { 
            if (GlobalVariables.BackendServer_Port.HasValue)
                CheckServerIsOnline(GlobalVariables.BackendServer_IP, GlobalVariables.BackendServer_Port.Value);
            else
                CheckServerIsOnline(GlobalVariables.BackendServer_IP);
        }

        public bool TestCheckServerIsOnline(string ip)
        {
            return TestCheckServerIsOnline(ip, 80);
        }
        public bool TestCheckServerIsOnline(string ip, int port)
        {
            bool serverIsOnline = GlobalVariables.ServerIsOnline;
            CheckServerIsOnline(ip, port);
            var ret = GlobalVariables.ServerIsOnline;
            GlobalVariables.ServerIsOnline = serverIsOnline;
            return ret;
        }
    }
}
