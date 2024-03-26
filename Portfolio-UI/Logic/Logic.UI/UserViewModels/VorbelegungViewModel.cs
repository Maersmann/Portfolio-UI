using Aktien.Data.Types;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.UserModels;
using CommunityToolkit.Mvvm.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Logic.UI.UserViewModels
{
    public class VorbelegungViewModel : ViewModelStammdaten<VorbelegungModel, StammdatenTypes>
    {

        public VorbelegungViewModel()
        {
            Title = "Vorbelegung User";
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL + "/api/Vorbelegung", Data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    GlobalUserVariables.JahrVon = Data.JahrVon;
                    GlobalUserVariables.UserID = Data.UserID;
                    GlobalUserVariables.VorbelegungID = Data.ID;
                     WeakReferenceMessenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp().ToString());
                }
                else
                {
                    SendExceptionMessage("Vorbelegung konnte nicht gespeichert werden.");
                    return;
                }
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.vorbelegung;



      

        #region Bindings   
       
        public int JahrVon
        {
            get { return Data.JahrVon; }
            set
            {

                if (RequestIsWorking || !Equals(Data.JahrVon, value))
                {
                    ValidateInt(value);
                    Data.JahrVon = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

        #region Validate
        private bool ValidateInt(int jahrvon)
        {
            BaseValidierung Validierung = new();

            bool isValid = Validierung.ValidateZahl(jahrvon, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "JahrVon", validationErrors);
            return isValid;
        }

        #endregion

        protected override void OnActivated()
        {
            Data = new VorbelegungModel { JahrVon = GlobalUserVariables.JahrVon, UserID = GlobalUserVariables.UserID, ID = GlobalUserVariables.VorbelegungID };
            JahrVon = GlobalUserVariables.JahrVon;
        }
    }
}
