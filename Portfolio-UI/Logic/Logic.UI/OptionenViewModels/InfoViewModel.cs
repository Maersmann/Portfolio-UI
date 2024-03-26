using Aktien.Logic.Core;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.OptionenModels;
using Logic.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Logic.UI.OptionenViewModels
{
    public class InfoViewModel : ViewModelBasis
    {
        public InfoViewModel()
        {
            Info = new InfoModel();
            Title = "Info";
        }
        public async Task LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + "/api/info");
                if (resp.IsSuccessStatusCode)
                    Info = await resp.Content.ReadAsAsync<InfoModel>();
                RequestIsWorking = false;
            }
        }

        public async void SetInfos(string version, DateTime release)
        {
            await LoadData();
            Info.ReleaseFronted = release;
            Info.VersionFrontend = version;

            OnPropertyChanged("Info");
        }

        public InfoModel Info { get; set; }
    }
}
