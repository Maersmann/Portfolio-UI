using Aktien.Data.Types;
using Aktien.Logic.Core;
using Base.Logic.Core;
using Base.Logic.Types;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using Data.Model.SteuerModels;
using Data.Types.SteuerTypes;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Logic.Core.SteuernLogic;
using Logic.Messages.SteuernMessages;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Logic.UI.SteuerViewModels
{
    public class SteuernUebersichtViewModel : ViewModelOfflineUebersicht<SteuerModel, StammdatenTypes>
    {
        private Action<bool, IList<SteuerModel>> callback;

        public SteuernUebersichtViewModel()
        {
            Title = "Übersicht Steuern";
        }

        protected override int GetID() { return SelectedItem.ID; }
        protected override StammdatenTypes GetStammdatenTyp() { return StammdatenTypes.steuer; }

        public void SetCallback(Action<bool, IList<SteuerModel>> callback)
        {
            this.callback = callback;
        }

        public void setSteuern(IList<SteuerModel> steuern)
        {
            steuern.ToList().ForEach(steuer =>
            {
                ItemList.Add(steuer);
            });
            if (ItemList.Count > 0)
            {
                SelectedItem = ItemList.First();
            }

        }

        #region Commands


        protected override void ExecuteEntfernenCommand()
        {
            SendInformationMessage("Steuer gelöscht");
            base.ExecuteEntfernenCommand();
        }

        public void ExceuteCallBack()
        {
            if (ItemList.Count > 0)
            {
                callback(true, ItemList.ToList());
            }
            else
                callback(false, null);

        }

        private void OpenSteuerStammdatenMessageCallback(bool confirmed, SteuerModel steuer)
        {
            if(confirmed)
            {
                ItemList.Add(steuer);
                RaisePropertyChanged(nameof(ItemList));
            }
        }


        protected override void ExecuteNeuCommand()
        {
            Messenger.Default.Send(new OpenSteuerStammdatenMessage<StammdatenTypes>(OpenSteuerStammdatenMessageCallback,null, SteuerartHelper.ErmittelAusSteuern(ItemList)) { State = State.Neu, ID = null, Stammdaten = GetStammdatenTyp()}, "SteuernUebersicht");
        }

        protected override void ExecuteBearbeitenCommand()
        {
            Messenger.Default.Send(new OpenSteuerStammdatenMessage<StammdatenTypes>(OpenSteuerStammdatenMessageCallback, SelectedItem, SteuerartHelper.ErmittelAusSteuern(ItemList)) { State = State.Bearbeiten, ID = SelectedItem.ID, Stammdaten = GetStammdatenTyp()}, "SteuernUebersicht");
        }


        #endregion


    }
}
