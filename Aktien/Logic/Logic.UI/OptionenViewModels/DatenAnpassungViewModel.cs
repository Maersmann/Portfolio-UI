using Aktien.Data.Types;
using Aktien.Logic.Core.DepotLogic;
using Aktien.Logic.Core.DepotLogic.Classes;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Messages.AuswahlMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using Aktien.Logic.UI.OptionenViewModels.Models;
using Aktien.Data.Model.DepotEntitys;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.OptionenViewModels
{
    public class DatenAnpassungViewModel : ViewModelBasis
    {
        private WertpapierBuyInModel BuyInModel;
        public DatenAnpassungViewModel()
        {
            Title = "Anpassungen";
            SpeicherBuyInCommand = new DelegateCommand( ExecuteSpeicherBuyInCommand, CanExecuteSpeicherBuyInCommand);
            AuswahlBuyInAktie = new RelayCommand(() => ExecuteAuswahlBuyInAktie());
            BuyInModel = new WertpapierBuyInModel { AlterBuyIn = 0, NeuerBuyIn = 0, DepotWertpapierID = 0, WertpapierName = "<<Nicht ausgewählt>>" };
            this.RaisePropertyChanged("WertpapierBuyInModel");
        }

        #region Bindings
        public ICommand SpeicherBuyInCommand { get; set; }
        public ICommand AuswahlBuyInAktie { get; set; }
        public WertpapierBuyInModel WertpapierBuyInModel { get => BuyInModel; }
        #endregion

        #region Commands
        private void ExecuteSpeicherBuyInCommand()
        {
            new DepotWertpapierAPI().Aktualisieren(BuyInModel.DepotWertpapier);
            SendInformationMessage("Erledigt");
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.buysell);
            BuyInModel = new WertpapierBuyInModel { AlterBuyIn = 0, NeuerBuyIn = 0, DepotWertpapierID = 0, WertpapierName = "<<Nicht ausgewählt>>" };
            this.RaisePropertyChanged("WertpapierBuyInModel");
            ((DelegateCommand)SpeicherBuyInCommand).RaiseCanExecuteChanged();
        }
        private bool CanExecuteSpeicherBuyInCommand()
        {
            return BuyInModel.DepotWertpapierID != 0;
        }
        private void ExecuteAuswahlBuyInAktie()
        {
            Messenger.Default.Send<OpenWertpapierAuswahlMessage>(new OpenWertpapierAuswahlMessage(OpenOpenWertpapierAuswahlMessageCallback), "DatenAnpassung");
        }
        #endregion

        #region Callbacks
        private void OpenOpenWertpapierAuswahlMessageCallback(bool confirmed, int id)
        {
            if (confirmed)
            {
                BuyInModel.DepotWertpapier = new DepotWertpapierAPI().LadeByWertpapierID(id);
                BuyInModel.AlterBuyIn = BuyInModel.DepotWertpapier.BuyIn;
                BuyInModel.DepotWertpapierID = BuyInModel.DepotWertpapier.ID;
                BuyInModel.WertpapierName = BuyInModel.DepotWertpapier.Wertpapier.Name;
                new DepotWertpapierFunctions().NeuBerechnen(BuyInModel.DepotWertpapier);
                BuyInModel.NeuerBuyIn = BuyInModel.DepotWertpapier.BuyIn;
                this.RaisePropertyChanged("WertpapierBuyInModel");
                ((DelegateCommand)SpeicherBuyInCommand).RaiseCanExecuteChanged();
            }
        }
        #endregion
    }
}
