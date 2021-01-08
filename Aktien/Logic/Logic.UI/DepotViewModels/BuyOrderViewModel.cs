using Aktien.Data.Types;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Core.Depot;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aktien.Data.Model.DepotModels;

namespace Aktien.Logic.UI.DepotViewModels
{
    public class BuyOrderViewModel : ViewModelStammdaten
    {
        private readonly OrderHistory data;
        public BuyOrderViewModel()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);

            state = State.Neu;
            data = new OrderHistory();

            KaufTyp = Data.Types.KaufTypes.Kauf;
            OrderTyp = Data.Types.OrderTypes.Normal;
            Anzahl = null;
            Preis = null;
            Datum = DateTime.Now;
        }

        protected override void ExecuteSaveCommand()
        {
            var Depot = new DepotAPI();
            Depot.NeuAktieGekauft(data.Preis, data.Fremdkostenzuschlag, data.Kaufdatum, AktieID , data.Anzahl, data.KaufartTyp, data.OrderartTyp);
            Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Buy-Order erfolgreich gespeichert." });
        }

        protected override void ExecuteCloseCommand()
        {
            ViewModelLocator.CleanUpBuyOrderView();
        }

        #region Bindings

        public IEnumerable<KaufTypes> KaufTypes
        {
            get
            {
                return Enum.GetValues(typeof(KaufTypes)).Cast<KaufTypes>();
            }
        }
        public IEnumerable<OrderTypes> OrderTypes
        {
            get
            {
                return Enum.GetValues(typeof(OrderTypes)).Cast<OrderTypes>();
            }
        }
        public KaufTypes KaufTyp
        {
            get { return data.KaufartTyp; }
            set
            {
                if ((this.data.KaufartTyp != value))
                {
                    this.data.KaufartTyp = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public OrderTypes OrderTyp
        {
            get { return data.OrderartTyp; }
            set
            {
                if ((this.data.OrderartTyp != value))
                {
                    this.data.OrderartTyp = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public int? Anzahl
        {
            get
            {
                return data.Anzahl;
            }
            set
            {
                if ( ValidateAnzahl(value) || (this.data.Anzahl != value))
                {
                    this.data.Anzahl = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public double? Fremdkosten
        {
            get
            {
                return data.Fremdkostenzuschlag;
            }
            set
            {
                if (this.data.Fremdkostenzuschlag != value)
                {
                    this.data.Fremdkostenzuschlag = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public double? Preis
        {
            get
            {
                return data.Preis;
            }
            set
            {
                if (ValidateBetrag(value) || (this.data.Preis != value))
                {
                    this.data.Preis = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public DateTime? Datum
        {
            get
            {
                return data.Kaufdatum;
            }
            set
            {
                if (ValidateDatum(value) || !DateTime.Equals(this.data.Kaufdatum, value))
                {
                    this.data.Kaufdatum = value.GetValueOrDefault();
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public int AktieID { get; set; }
        #endregion



        #region Validierung

        private bool ValidateAnzahl(int? inAnzahl)
        {
            const string propertyKey = "Anzahl";
            var Validierung = new AktieGekauftValidierung();

            bool isValid = Validierung.ValidateAnzahl(inAnzahl, out ICollection<string> validationErrors);


            if (!isValid)
            {

                ValidationErrors[propertyKey] = validationErrors;

                RaiseErrorsChanged(propertyKey);
            }
            else if (ValidationErrors.ContainsKey(propertyKey))
            {

                ValidationErrors.Remove(propertyKey);

                RaiseErrorsChanged(propertyKey);
            }
            return isValid;
        }

        private bool ValidateBetrag(Double? inBetrag)
        {
            const string propertyKey = "Preis";
            var Validierung = new AktieGekauftValidierung();

            bool isValid = Validierung.ValidateBetrag(inBetrag, out ICollection<string> validationErrors);


            if (!isValid)
            {

                ValidationErrors[propertyKey] = validationErrors;

                RaiseErrorsChanged(propertyKey);
            }
            else if (ValidationErrors.ContainsKey(propertyKey))
            {

                ValidationErrors.Remove(propertyKey);

                RaiseErrorsChanged(propertyKey);
            }
            return isValid;
        }

        private bool ValidateDatum(DateTime? value)
        {
            const string propertyKey = "Datum";
            var Validierung = new AktieGekauftValidierung();

            bool isValid = Validierung.ValidateDatum(value, out ICollection<string> validationErrors);


            if (!isValid)
            {

                ValidationErrors[propertyKey] = validationErrors;

                RaiseErrorsChanged(propertyKey);
            }
            else if (ValidationErrors.ContainsKey(propertyKey))
            {

                ValidationErrors.Remove(propertyKey);

                RaiseErrorsChanged(propertyKey);
            }
            return isValid;
        }

        #endregion
    }
}
