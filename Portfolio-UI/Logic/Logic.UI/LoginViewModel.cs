using Aktien.Data.Types;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using Data.Model.UserModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Base;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Logic.UI
{
    public class LoginViewModel : ViewModelValidate
    {
        readonly AuthenticateModel authenticate;
        public LoginViewModel()
        {
            Title = "Anmeldung";
            LoginCommand = new DelegateCommand(ExecuteLoginCommand, CanExecuteCommand);
            PasswordCommand = new RelayCommand<PasswordBox>(ExecutePasswordChangedCommand);

            authenticate = new AuthenticateModel();
            Password = "";
            User = "";
        }

        private async void ExecuteLoginCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;

                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + "/api/Users/authenticate", authenticate);

                RequestIsWorking = false;
                if (resp.IsSuccessStatusCode)
                {
                    AuthenticateResponseModel Response = await resp.Content.ReadAsAsync<AuthenticateResponseModel>();
                    GlobalVariables.Token = Response.Token;
                    await LoadingVorbelegung(Response.Id);
                    Messenger.Default.Send(new AktualisiereBerechtigungenMessage());
                    Messenger.Default.Send(new OpenViewMessage { ViewType = ViewType.viewWertpapierUebersicht });
                    Messenger.Default.Send(new CloseViewMessage(), "Login");
                }
                else if (resp.StatusCode == System.Net.HttpStatusCode.BadRequest)
                { 
                    SendExceptionMessage("User oder Passwort ist falsch");
                }
            }
        }

        protected bool CanExecuteCommand()
        {
            return ValidationErrors.Count == 0;
        }

        protected override void ExecuteCleanUpCommand()
        {
            base.ExecuteCleanUpCommand();
            if ( string.IsNullOrEmpty(GlobalVariables.Token))
            {
                Messenger.Default.Send(new CloseApplicationMessage());
            }
        }

        private async Task LoadingVorbelegung(int userid)
        {
            if (GlobalVariables.ServerIsOnline)
            {
                SetConnection();
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/Vorbelegung/{userid}");
                if (resp.IsSuccessStatusCode)
                {
                    Response<VorbelegungModel> Response = await resp.Content.ReadAsAsync<Response<VorbelegungModel>>();
                    GlobalUserVariables.JahrVon = Response.Data.JahrVon;
                    GlobalUserVariables.VorbelegungID = Response.Data.ID;
                    GlobalUserVariables.UserID = Response.Data.UserID;
                }
            }
        }


        #region Bindings
        public ICommand LoginCommand { get; private set; }

        public string User
        {
            get { return authenticate.Username; }
            set
            {
                if (RequestIsWorking || !string.Equals(authenticate.Username, value))
                {
                    ValidateName(value);
                    authenticate.Username = value;
                    RaisePropertyChanged();
                    ((DelegateCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get { return authenticate.Password; }
            set
            {
                if (RequestIsWorking || !string.Equals(authenticate.Password, value))
                {
                    ValidatePassword(value);
                    authenticate.Password = value;
                    RaisePropertyChanged();
                    ((DelegateCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand<PasswordBox> PasswordCommand { get; private set; }


        private void ExecutePasswordChangedCommand(PasswordBox obj)
        {
            if (obj != null)
                Password = obj.Password;
        }
        #endregion

        #region Validate
        private bool ValidateName(string name)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(name, "Der User", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, nameof(User), validationErrors);
            return isValid;
        }
        private bool ValidatePassword(string name)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(name, "Das Password", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, nameof(Password), validationErrors);
            return isValid;
        }
        #endregion
    }
}
