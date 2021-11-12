using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Base.Logic.ViewModels;
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
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;
using Base.Logic.Wrapper;

namespace Aktien.Logic.UI.ETFViewModels
{
    public class ETFStammdatenViewModel : ViewModelStammdaten<ETFModel, StammdatenTypes>, IViewModelStammdaten
    {

        public ETFStammdatenViewModel()
        {
            Title = "Informationen ETF";
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PostAsJsonAsync(GlobalVariables.BackendServer_URL+"/api/etf", Data);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
                    Messenger.Default.Send(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());
                }
                else if ((int)resp.StatusCode == 904)
                {
                    SendExceptionMessage("ETF ist schon vorhanden");
                    return;
                }
                else
                {
                    SendExceptionMessage("ETF konnte nicht gespeichert werden.");
                    return;
                }
            }
        }
        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.etf;

        public async void ZeigeStammdatenAn(int id)
        {
            RequestIsWorking = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+$"/api/etf/{id}");
                if (resp.IsSuccessStatusCode)
                    Response = await resp.Content.ReadAsAsync<Response<ETFModel>>();
            }

            ProfitTyp = Data.ProfitTyp;
            ErmittentTyp = Data.Emittent;
            WKN = Data.WKN;
            Name = Data.Name;
            ISIN = Data.ISIN;

            RequestIsWorking = false;
            state = State.Bearbeiten;
            RaisePropertyChanged("ISIN_isEnabled");
        }


        public bool ISIN_isEnabled => state == State.Neu;

        #region Bindings   
        public string ISIN
        {
            get => Data.ISIN;
            set
            {

                if (RequestIsWorking || !string.Equals(Data.ISIN, value))
                {
                    ValidateISIN(value);
                    Data.ISIN = value;
                    RaisePropertyChanged();
                    (SaveCommand as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string Name
        {
            get => Data.Name;
            set
            {
                if (RequestIsWorking || !string.Equals(Data.Name, value))
                {
                    ValidateName(value);
                    Data.Name = value;
                    RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string WKN
        {
            get => Data.WKN;
            set
            {

                if (RequestIsWorking || !string.Equals(Data.WKN, value))
                {
                    Data.WKN = value;
                    RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<ProfitTypes> ProfitTypes => Enum.GetValues(typeof(ProfitTypes)).Cast<ProfitTypes>();
        public ProfitTypes ProfitTyp
        {
            get => Data.ProfitTyp;
            set
            {
                if (RequestIsWorking || (Data.ProfitTyp != value))
                {
                    Data.ProfitTyp = value;
                    RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<ErmittentTypes> ErmittentTypes => Enum.GetValues(typeof(ErmittentTypes)).Cast<ErmittentTypes>();
        public ErmittentTypes ErmittentTyp
        {
            get => Data.Emittent;
            set
            {
                if (RequestIsWorking || (Data.Emittent != value))
                {
                    Data.Emittent = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Validate
        private bool ValidateName(String name)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(name, "Der Name", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Name", validationErrors);
            return isValid;
        }

        private bool ValidateISIN(String isin)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateString(isin, "Die ISIN", out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "ISIN", validationErrors);
            return isValid;
        }
        #endregion

        public override void Cleanup()
        {
            state = State.Neu;
            Data = new ETFModel();
            ISIN = "";
            Name = "";
            WKN = "";
            ProfitTyp = Aktien.Data.Types.WertpapierTypes.ProfitTypes.Thesaurierend;
            ErmittentTyp = Aktien.Data.Types.WertpapierTypes.ErmittentTypes.iShares;
        }
    }
}
