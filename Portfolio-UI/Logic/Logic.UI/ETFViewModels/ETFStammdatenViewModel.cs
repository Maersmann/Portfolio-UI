using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using Aktien.Logic.UI.InterfaceViewModels;
using Data.Model.ETFModels;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.ETFViewModels
{
    public class ETFStammdatenViewModel : ViewModelStammdaten<ETFModel>, IViewModelStammdaten
    {

        public ETFStammdatenViewModel()
        {
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Cleanup();
            Title = "Informationen ETF";
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.PostAsJsonAsync("https://localhost:5001/api/etf", data);


                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), GetStammdatenTyp());
                }
                else if (resp.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    SendExceptionMessage("ETF ist schon vorhanden");
                    return;
                }
                else
                {
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                }
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.etf;




        public async void ZeigeStammdatenAn(int id)
        {
            LoadAktie = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync($"https://localhost:5001/api/etf/{id}");
                if (resp.IsSuccessStatusCode)
                    data = await resp.Content.ReadAsAsync<ETFModel>();
            }

            ProfitTyp = data.ProfitTyp;
            ErmittentTyp = data.Emittent; 
            WKN = data.WKN;
            Name = data.Name;
            ISIN = data.ISIN;

            LoadAktie = false;
            state = State.Bearbeiten;
            this.RaisePropertyChanged("ISIN_isEnabled"); 
        }


        public bool ISIN_isEnabled { get { return state == State.Neu; } }

        #region Bindings   
        public string ISIN
        {
            get { return this.data.ISIN; }
            set
            {

                if (LoadAktie || !string.Equals(this.data.ISIN, value))
                {
                    ValidateISIN(value);
                    this.data.ISIN = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string Name
        {
            get { return this.data.Name; }
            set
            {
                if (LoadAktie || !string.Equals(this.data.Name, value))
                {
                    ValidateName(value);
                    this.data.Name = value;
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string WKN
        {
            get { return this.data.WKN; }
            set
            {

                if (LoadAktie || !string.Equals(this.data.WKN, value))
                {
                    this.data.WKN = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<ProfitTypes> ProfitTypes
        {
            get
            {
                return Enum.GetValues(typeof(ProfitTypes)).Cast<ProfitTypes>();
            }
        }
        public ProfitTypes ProfitTyp
        {
            get { return data.ProfitTyp; }
            set
            {
                if (LoadAktie || (this.data.ProfitTyp != value))
                {
                    this.data.ProfitTyp = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<ErmittentTypes> ErmittentTypes
        {
            get
            {
                return Enum.GetValues(typeof(ErmittentTypes)).Cast<ErmittentTypes>();
            }
        }
        public ErmittentTypes ErmittentTyp
        {
            get { return data.Emittent; }
            set
            {
                if (LoadAktie || (this.data.Emittent != value))
                {
                    this.data.Emittent = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Validate
        private bool ValidateName(String name)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(name, "Name", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Name", validationErrors);
            return isValid;
        }

        private bool ValidateISIN(String isin)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(isin, "ISIN", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "ISIN", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            state = State.Neu;
            data = new ETFModel();
            ISIN = "";
            Name = "";
            WKN = "";
            ProfitTyp = Data.Types.WertpapierTypes.ProfitTypes.Thesaurierend;
            ErmittentTyp = Data.Types.WertpapierTypes.ErmittentTypes.iShares;
        }
    }
}
