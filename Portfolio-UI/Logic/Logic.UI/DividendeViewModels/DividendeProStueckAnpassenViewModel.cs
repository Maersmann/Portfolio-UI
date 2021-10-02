using Aktien.Data.Types;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.Base;
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.ViewModels;
using Data.Model.DividendeModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeProStueckAnpassenViewModel : ViewModelStammdaten<DividendeProStueckAnpassenModel, StammdatenTypes>
    {

        public DividendeProStueckAnpassenViewModel()
        {
            Title = "Dividende pro Stück";
            data = new DividendeProStueckAnpassenModel();
            RundungTyp = DividendenRundungTypes.Normal;
            OKCommand = new RelayCommand(() => ExecuteOKCommand());
        }

        private async void ExecuteOKCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.PutAsJsonAsync(GlobalVariables.BackendServer_URL+ "/api/dividende/Rundung", data as DividendeProStueckAnpassenDTOModel);
                RequestIsWorking = false;

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende aktualisiert." }, "DividendeProStueckAnpassen");
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), GetStammdatenTyp().ToString());
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
                    DividendeModel dividende = await resp.Content.ReadAsAsync<DividendeModel>();
                    data.DividendeID = dividende.ID;
                    data.Rundungart = dividende.RundungArt;
                    data.Zahldatum = dividende.Zahldatum;
                    data.Betrag = dividende.Betrag;
                    data.Waehrung = dividende.Waehrung;
                }
                RequestIsWorking = false;
            }
            data.Umrechnungskurs = umrechungskurs;
            RaisePropertyChanged("Datum");
            RaisePropertyChanged("Betrag");
            RaisePropertyChanged("Waehrung");
            RaisePropertyChanged("Umrechnungskurs");
            RaisePropertyChanged("ErmittelterBetrag");
            RaisePropertyChanged("ErhaltenerBetrag");
            RaisePropertyChanged("RundungTyp");
        }

        #region Bindings

        public ICommand OKCommand { get; set; }

        public DateTime Datum => data.Zahldatum;
        public double Betrag => data.Betrag;
        public Waehrungen Waehrung => data.Waehrung;
        public double Umrechnungskurs => data.Umrechnungskurs;

        public double ErmittelterBetrag => new DividendenBerechnungen().BetragUmgerechnet(data.Betrag, data.Umrechnungskurs, false, DividendenRundungTypes.Normal);
        public double ErhaltenerBetrag => new DividendenBerechnungen().BetragUmgerechnet(data.Betrag, data.Umrechnungskurs, true, data.Rundungart);

        public IEnumerable<DividendenRundungTypes> RundungTypes => Enum.GetValues(typeof(DividendenRundungTypes)).Cast<DividendenRundungTypes>();
        public DividendenRundungTypes RundungTyp
        {
            get { return data.Rundungart; }
            set
            {
                if (data.Rundungart != value)
                {
                    data.Rundungart = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("ErhaltenerBetrag");
                }
            }
        }

        #endregion
    }
}
