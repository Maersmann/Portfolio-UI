using Aktien.Logic.Core;
using Aktien.Logic.UI.BaseViewModels;
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
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + "/api/info");
                if (resp.IsSuccessStatusCode)
                    Info = await resp.Content.ReadAsAsync<InfoModel>();
            }
            Info.VersionFrontend = new VersionHelper().GetVersion;
            Info.ReleaseFronted = new VersionHelper().GetRelease;
            RaisePropertyChanged("Info");
        }

        public InfoModel Info { get; set; }
    }
}
