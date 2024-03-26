using Aktien.Data.Types;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
using Base.Logic.Wrapper;
using Data.Model.DividendeModels;

using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeProStueckAnpassenViewModel : ViewModelStammdaten<DividendeProStueckAnpassenModel, StammdatenTypes>
    {

        public DividendeProStueckAnpassenViewModel()
        {
            Title = "Dividende pro Stück";
            Data = new DividendeProStueckAnpassenModel();
            RundungTyp = DividendenRundungTypes.Normal;
            OKCommand = new RelayCommand(() => ExecuteOKCommand());
        }

        private async void ExecuteOKCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PutAsJsonAsync(GlobalVariables.BackendServer_URL+ "/api/dividende/Rundung", Data as DividendeProStueckAnpassenDTOModel);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                     WeakReferenceMessenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende aktualisiert." }, "DividendeProStueckAnpassen");
                }
                else if (resp.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    SendExceptionMessage("Fehler - Dividende pro Stück");
                    return;
                }
            }
            
        }

        public async void LoadData(int dividendeID, double umrechungskurs)
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL+ $"/api/dividende/{dividendeID}");
                if (resp.IsSuccessStatusCode)
                {
                    Response<DividendeModel> DividendeResponse = await resp.Content.ReadAsAsync<Response<DividendeModel>>();
                    Data.DividendeID = DividendeResponse.Data.ID;
                    Data.Rundungart = DividendeResponse.Data.RundungArt;
                    Data.Zahldatum = DividendeResponse.Data.Zahldatum;
                    Data.Betrag = DividendeResponse.Data.Betrag;
                    Data.Waehrung = DividendeResponse.Data.Waehrung;
                }
                RequestIsWorking = false;
            }
            Data.Umrechnungskurs = umrechungskurs;
            OnPropertyChanged("Datum");
            OnPropertyChanged("Betrag");
            OnPropertyChanged("Waehrung");
            OnPropertyChanged("Umrechnungskurs");
            OnPropertyChanged("ErmittelterBetrag");
            OnPropertyChanged("ErhaltenerBetrag");
            OnPropertyChanged("RundungTyp");
        }

        #region Bindings

        public ICommand OKCommand { get; set; }

        public DateTime Datum => Data.Zahldatum;
        public double Betrag => Data.Betrag;
        public Waehrungen Waehrung => Data.Waehrung;
        public double Umrechnungskurs => Data.Umrechnungskurs;

        public double ErmittelterBetrag => new DividendenBerechnungen().BetragUmgerechnet(Data.Betrag, Data.Umrechnungskurs, false, DividendenRundungTypes.Normal);
        public double ErhaltenerBetrag => new DividendenBerechnungen().BetragUmgerechnet(Data.Betrag, Data.Umrechnungskurs, true, Data.Rundungart);

        public IEnumerable<DividendenRundungTypes> RundungTypes => Enum.GetValues(typeof(DividendenRundungTypes)).Cast<DividendenRundungTypes>();
        public DividendenRundungTypes RundungTyp
        {
            get { return Data.Rundungart; }
            set
            {
                if (Data.Rundungart != value)
                {
                    Data.Rundungart = value;
                    OnPropertyChanged();
                    OnPropertyChanged("ErhaltenerBetrag");
                }
            }
        }

        #endregion
    }
}
