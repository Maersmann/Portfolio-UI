using Aktien.Logic.Core;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.OptionenModels;
using Logic.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Logic.UI.OptionenViewModels
{
    public class InfoViewModel : ViewModelBasis
    {
        public InfoViewModel()
        {
            Info = new InfoModel();
            Title = "Info";
            LoadData();
        }
        public async void LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + "/api/info");
                if (resp.IsSuccessStatusCode)
                    Info = await resp.Content.ReadAsAsync<InfoModel>();
                RequestIsWorking = false;
            }
            Info.VersionFrontend = new VersionHelper().GetVersion;
            Info.ReleaseFronted = new VersionHelper().GetRelease;
            RaisePropertyChanged("Info");
        }

        public InfoModel Info { get; set; }
    }
}
