using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Core.WertpapierLogic;
using Aktien.Logic.Messages.AktieMessages;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.Messages.DepotMessages;
using Aktien.Logic.Messages.WertpapierMessages;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.WertpapierViewModels
{
    public class OrderUebersichtViewModel : ViewModelUebersicht<OrderHistory>
    {

        private int wertpapierID;
        private WertpapierTypes wertpapierTypes;
        private string messagtoken;

        public OrderUebersichtViewModel()
        {
            Title = "Übersicht der Order";
            wertpapierID = 0;
            wertpapierTypes = WertpapierTypes.Aktie;
            AktieGekauftCommand = new DelegateCommand(this.ExecuteAktieGekauftCommand, this.CanExecuteCommand);
            AktieVerkauftCommand = new DelegateCommand(this.ExecuteAktieVerkauftCommand, this.CanExecuteAktieVerkaufCommand);
            EntfernenCommand = new DelegateCommand(this.ExecuteEntfernenCommand, this.CanSelectedItemExecuteCommand);
            RegisterAktualisereViewMessage(StammdatenTypes.buysell);
        }

        public override string MessageToken
        {
            set
            {
                Messenger.Default.Register<LoadWertpapierOrderMessage>(this, value, m => ReceiveLoadAktieMessage(m));
                messagtoken = value;
            }
        }


        private void ReceiveLoadAktieMessage(LoadWertpapierOrderMessage m)
        {
            wertpapierTypes = m.WertpapierTyp;
            LoadData(m.WertpapierID);
        }

        public override void LoadData()
        {
            LoadData(wertpapierID);
        }

        public override void LoadData(int id)
        {
            wertpapierID = id;
            itemList = new AktieAPI().LadeAlleOrdersDerAktie(wertpapierID);
            this.RaisePropertyChanged("ItemList");
            ((DelegateCommand)AktieGekauftCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)AktieVerkauftCommand).RaiseCanExecuteChanged();

        }

        #region Bindings

        public ICommand AktieGekauftCommand { get; set; }
        public ICommand AktieVerkauftCommand { get; set; }

        #endregion

        #region Commands
        private void ExecuteAktieGekauftCommand()
        {
            Messenger.Default.Send<OpenAktieGekauftViewMessage>(new OpenAktieGekauftViewMessage { WertpapierID = wertpapierID, BuySell = BuySell.Buy, WertpapierTypes = wertpapierTypes }, messagtoken);
        }
        private void ExecuteAktieVerkauftCommand()
        {
            Messenger.Default.Send<OpenAktieGekauftViewMessage>(new OpenAktieGekauftViewMessage { WertpapierID = wertpapierID, BuySell = BuySell.Sell, WertpapierTypes = wertpapierTypes }, messagtoken);
        }
        protected override void ExecuteEntfernenCommand()
        {
            if (selectedItem.BuySell == BuySell.Buy)
            {
                new DepotAPI().EntferneGekauftenWertpapier(selectedItem.ID);
            }
            else
            {
                new DepotAPI().EntferneVerkauftenWertpapier(selectedItem.ID);
            }
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), ViewType.viewDepotUebersicht);
            base.ExecuteEntfernenCommand();
        }

        protected override bool CanExecuteCommand()
        {
            return wertpapierID != 0;
        }
        private bool CanSelectedItemExecuteCommand()
        {
            return (selectedItem != null) && ( itemList.IndexOf(SelectedItem) == 0 );
        }

        private bool CanExecuteAktieVerkaufCommand()
        {
            return new DepotAPI().WertpapierImDepotVorhanden( wertpapierID ); 
        }

        #endregion

    }
}
