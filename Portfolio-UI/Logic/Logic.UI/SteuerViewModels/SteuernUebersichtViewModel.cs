using Aktien.Data.Types;
using Aktien.Logic.Core;
using Aktien.Logic.UI.BaseViewModels;
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
    public class SteuernUebersichtViewModel : ViewModelUebersicht<SteuerModel>
    {
        private Action<bool, int> callback;
        private int? steuergruppeID;
        private SteuerHerkunftTyp steuerherkunfttyp;
        private bool istVerknuepfungGespeichert;

        public SteuernUebersichtViewModel()
        {
            Title = "Übersicht Steuern";
            steuergruppeID = null;
            istVerknuepfungGespeichert = false;
            steuerherkunfttyp = SteuerHerkunftTyp.shtDividende;
            LoadData();
            RegisterAktualisereViewMessage(StammdatenTypes.steuer);
        }

        public void setHerkunftTyp(SteuerHerkunftTyp steuerHerkunftTyp)
        {
            this.steuerherkunfttyp = steuerHerkunftTyp;
        }
        protected override int GetID() { return selectedItem.ID; }
        protected override StammdatenTypes GetStammdatenType() { return StammdatenTypes.steuer; }
        public void SetCallback(Action<bool, int> callback)
        {
            this.callback = callback;
        }
        public async override void LoadData(int steuergruppeID)
        {
            this.steuergruppeID = steuergruppeID;

            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync($"https://localhost:5001/api/Steuern?steuergruppeid={steuergruppeID}");
                if (resp.IsSuccessStatusCode)
                    itemList = await resp.Content.ReadAsAsync<ObservableCollection<SteuerModel>>();
                else
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
            }
            this.RaisePropertyChanged("ItemList");
        }

        #region Commands

        protected async override void ExecuteEntfernenCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.DeleteAsync($"https://localhost:5001/api/Steuern/{selectedItem.ID}");
                if (!resp.IsSuccessStatusCode)
                {
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                    return;
                }

            }
            SendInformationMessage("Steuer gelöscht");
            base.ExecuteEntfernenCommand();
        }

        public void ExceuteCallBack()
        {
            if (steuergruppeID.HasValue)
                callback(true, this.steuergruppeID.Value);
            else
                callback(false, 0);

        }


        public void IstVerknuepfungGespeichert(bool istVerknuepfungGespeichert)
        {
            this.istVerknuepfungGespeichert = istVerknuepfungGespeichert;
        }

        protected override void ExecuteNeuCommand()
        {
            Messenger.Default.Send<OpenSteuerStammdatenMessage>(new OpenSteuerStammdatenMessage { State = State.Neu, ID = null, StammdatenTyp = GetStammdatenType(), SteuergruppeID = this.steuergruppeID, Typ = steuerherkunfttyp, IstVerknuepfungGespeichert = istVerknuepfungGespeichert },"SteuernUebersicht"); 
        }

        protected override void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send<OpenSteuerStammdatenMessage>(new OpenSteuerStammdatenMessage { State = State.Bearbeiten, ID = selectedItem.ID, StammdatenTyp = GetStammdatenType(), SteuergruppeID = this.steuergruppeID, Typ = steuerherkunfttyp, IstVerknuepfungGespeichert = istVerknuepfungGespeichert }, "SteuernUebersicht");
        }


        #endregion


    }
}
