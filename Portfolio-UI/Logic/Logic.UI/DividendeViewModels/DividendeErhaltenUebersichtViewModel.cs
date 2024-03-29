﻿using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.Messages.DividendeMessages;
using Base.Logic.Core;
using Base.Logic.Types;
using Base.Logic.ViewModels;
using Data.Model.DividendeModels;
using CommunityToolkit.Mvvm.Messaging;
using Logic.Messages.DividendeMessages;
using Logic.Messages.UtilMessages;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeErhaltenUebersichtViewModel : ViewModelUebersicht<DividendeErhaltenUebersichtModel, StammdatenTypes>
    {

        private int wertpapierID;

        public DividendeErhaltenUebersichtViewModel()
        {
            Title = "Übersicht aller erhaltene Dividenden";
            RegisterAktualisereViewMessage(StammdatenTypes.dividendeErhalten.ToString());
            OpenReitAktualisierungCommand = new RelayCommand(() => ExecuteOpenReitAktualisierungCommand());
        }
        protected override string GetREST_API() { return $"/api/Wertpapier/{wertpapierID}/ErhalteneDividenden/"; }
        protected override bool WithPagination() { return true; }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.dividendeErhalten;

        public  override void LoadData(int id)
        {
            wertpapierID = id;
            base.LoadData(id);
        }

        #region Commands
        protected override void ExecuteNeuCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenErhaltendeDividendeStammdatenMessage<StammdatenTypes> { WertpapierID = wertpapierID, State = State.Neu });
        }
        protected override void ExecuteBearbeitenCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenErhaltendeDividendeStammdatenMessage<StammdatenTypes> { WertpapierID = wertpapierID, State = State.Bearbeiten, ID = SelectedItem.ID });
        }

        private void ExecuteOpenReitAktualisierungCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenDividendeReitAkualiserungMessage { ID = SelectedItem.ID }, "DividendeErhaltenUebersicht");
        }


        protected override void ExecuteEntfernenCommand()
        {
             WeakReferenceMessenger.Default.Send(new OpenBestaetigungViewMessage
            {
                Beschreibung = "Soll der Eintrag gelöscht werden?",
                Command = async () =>
                                {
                                    if (GlobalVariables.ServerIsOnline)
                                    {
                                        RequestIsWorking = true;
                                        HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/DividendeErhalten/{SelectedItem.ID}");
                                        RequestIsWorking = false;
                                        if (resp.IsSuccessStatusCode)
                                        {
                                            SendInformationMessage("Dividende Erhalten gelöscht");
                                            base.ExecuteEntfernenCommand();
                                        }
                                    }
                                }
            },"DividendeErhaltenUebersicht");
        }
        #endregion

        #region Bindings
        public ICommand OpenReitAktualisierungCommand { get; set; }
        #endregion
    }
}
