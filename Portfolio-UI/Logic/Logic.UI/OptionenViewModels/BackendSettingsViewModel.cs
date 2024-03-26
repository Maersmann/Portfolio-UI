using Aktien.Logic.Core;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using CommunityToolkit.Mvvm.Input;
using Data.Model.OptionenModels;
using Data.Types.OptionTypes;

using Logic.Core.OptionenLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.OptionenViewModels
{
    public class BackendSettingsViewModel : ViewModelBasis
    {
        private readonly BackendSettingsModel model;
        public BackendSettingsViewModel()
        {
            model = new BackendSettingsModel();
            SpeicherSettingsCommand = new RelayCommand(() => ExecuteSpeicherSettingsCommand());
            TestConnectionCommand = new RelayCommand(() => ExecuteTestConnectionCommand());
            Title = "Backend-Settings";
            setModelData();
        }

        public void setModelData()
        {
            BackendLogic backendLogic = new();
            if(backendLogic.istINIVorhanden())
            { 
                backendLogic.LoadData();
                model.Backend_IP = backendLogic.getBackendIP();
                model.ProtokollTyp = backendLogic.getProtokollTyp();
                model.Port = backendLogic.getBackendPort();
                model.Backend_URL = backendLogic.GetBackendURL();
            }
        }

        #region Bindings
        public ICommand SpeicherSettingsCommand { get; set; }
        public ICommand TestConnectionCommand { get; set; }
        public string Backend_IP
        {
            get => model.Backend_IP;
            set
            {
                model.Backend_IP = value;
                OnPropertyChanged();
            }
        }
        public string Backend_URL
        {
            get => model.Backend_URL;
            set
            {
                model.Backend_URL = value;
                OnPropertyChanged();
            }
        }
        public string Port
        {
            get => model.Port.HasValue ? model.Port.Value.ToString() : "";
            set
            {
                model.Port = value.Equals("") ? null : (int?)int.Parse(value);
                OnPropertyChanged();
            }
        }

        public static IEnumerable<BackendProtokollTypes> BackendProtokollTypes => Enum.GetValues(typeof(BackendProtokollTypes)).Cast<BackendProtokollTypes>();
        public BackendProtokollTypes BackendProtokollTyp
        {
            get { return model.ProtokollTyp; }
            set
            {
                this.model.ProtokollTyp = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        private void ExecuteSpeicherSettingsCommand()
        {
            BackendLogic backendlogic = new();
            backendlogic.SaveData(model.Backend_IP, model.ProtokollTyp, model.Port, model.Backend_URL);
            SendInformationMessage("Settings gespeichert");
            GlobalVariables.BackendServer_IP = backendlogic.getBackendIP();
            GlobalVariables.BackendServer_URL = backendlogic.getURL();
            GlobalVariables.BackendServer_Port = backendlogic.getBackendPort();
            BackendHelper.CheckServerIsOnline();
        }

        private void ExecuteTestConnectionCommand()
        {
            bool isOnline = model.Port.HasValue
                ? BackendHelper.TestCheckServerIsOnline(model.Backend_IP, model.Port.Value)
                : BackendHelper.TestCheckServerIsOnline(model.Backend_IP);
            if (isOnline)
            {
                SendInformationMessage("Test Verbindung erfolgreich");
            }
        }
        #endregion
    }
}
