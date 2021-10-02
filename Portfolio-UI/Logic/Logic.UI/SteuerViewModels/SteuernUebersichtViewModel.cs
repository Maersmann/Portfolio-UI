﻿using Aktien.Data.Types;
using Aktien.Logic.Core;
using Base.Logic.Core;
using Base.Logic.Types;
using Base.Logic.ViewModels;
using Data.Model.SteuerModels;
using Data.Types.SteuerTypes;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.SteuernMessages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace Logic.UI.SteuerViewModels
{
    public class SteuernUebersichtViewModel : ViewModelUebersicht<SteuerModel, StammdatenTypes>
    {
        private Action<bool, int?> callback;
        private int? steuergruppeID;
        private SteuerHerkunftTyp steuerherkunfttyp;
        private bool istVerknuepfungGespeichert;

        public SteuernUebersichtViewModel()
        {
            Title = "Übersicht Steuern";
            steuergruppeID = null;
            istVerknuepfungGespeichert = false;
            steuerherkunfttyp = SteuerHerkunftTyp.shtDividende;
            RegisterAktualisereViewMessage(StammdatenTypes.steuer.ToString());
        }

        public void setHerkunftTyp(SteuerHerkunftTyp steuerHerkunftTyp)
        {
            this.steuerherkunfttyp = steuerHerkunftTyp;
        }
        protected override int GetID() { return selectedItem.ID; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.steuer; }

        public void SetCallback(Action<bool, int?> callback)
        {
            this.callback = callback;
        }
        public async override void LoadData(int steuergruppeID)
        {
            this.steuergruppeID = steuergruppeID;

            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/Steuern?steuergruppeid={steuergruppeID}");
                if (resp.IsSuccessStatusCode)
                    itemList = await resp.Content.ReadAsAsync<ObservableCollection<SteuerModel>>();
                else
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                RequestIsWorking = false;
            }
            RaisePropertyChanged("ItemList");
        }

        #region Commands

        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.DeleteAsync(GlobalVariables.BackendServer_URL + $"/api/Steuern/{selectedItem.ID}");
                RequestIsWorking = false;
                if (!resp.IsSuccessStatusCode)
                {
                    SendExceptionMessage("Steuer konnte nicht gelöscht werden.");
                    return;
                }
                var respObj = await resp.Content.ReadAsAsync<bool>();
                if (respObj)
                    steuergruppeID = null;

            }
            SendInformationMessage("Steuer gelöscht");
            base.ExecuteEntfernenCommand();
        }

        public void ExceuteCallBack()
        {
            if (steuergruppeID.HasValue)
                callback(true, this.steuergruppeID.Value);
            else
                callback(false, null);

        }


        public void IstVerknuepfungGespeichert(bool istVerknuepfungGespeichert)
        {
            this.istVerknuepfungGespeichert = istVerknuepfungGespeichert;
        }

        protected override void ExecuteNeuCommand()
        {
            Messenger.Default.Send(new OpenSteuerStammdatenMessage<StammdatenTypes> { State = State.Neu, ID = null, Stammdaten = GetStammdatenTyp(), SteuergruppeID = this.steuergruppeID, Typ = steuerherkunfttyp, IstVerknuepfungGespeichert = istVerknuepfungGespeichert },"SteuernUebersicht"); 
        }

        protected override void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send(new OpenSteuerStammdatenMessage<StammdatenTypes> { State = State.Bearbeiten, ID = selectedItem.ID, Stammdaten = GetStammdatenTyp(), SteuergruppeID = this.steuergruppeID, Typ = steuerherkunfttyp, IstVerknuepfungGespeichert = istVerknuepfungGespeichert }, "SteuernUebersicht");
        }


        #endregion


    }
}
