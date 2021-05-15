﻿using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
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
    public class DividendeProStueckAnpassenViewModel : ViewModelStammdaten<DividendeModel>
    {
        private double umrechnungskurs;

        public DividendeProStueckAnpassenViewModel()
        {
            Title = "Dividende pro Stück";
            data = new DividendeModel();
            RundungTyp = DividendenRundungTypes.Normal;
            umrechnungskurs = 0;
            OKCommand = new RelayCommand(() => ExecuteOKCommand());
        }

        private async void ExecuteOKCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.PostAsJsonAsync("https://localhost:5001/api/dividende", data);

                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende aktualisiert." }, "DividendeProStueckAnpassen");
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), GetStammdatenTyp());
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
                HttpResponseMessage resp = await Client.GetAsync($"https://localhost:5001/api/dividende/{dividendeID}");
                if (resp.IsSuccessStatusCode)
                {
                    data = await resp.Content.ReadAsAsync<DividendeModel>();
                }
            }
            umrechnungskurs = umrechungskurs;
            this.RaisePropertyChanged("Datum");
            this.RaisePropertyChanged("Betrag");
            this.RaisePropertyChanged("Waehrung");
            this.RaisePropertyChanged("Umrechnungskurs");
            this.RaisePropertyChanged("ErmittelterBetrag");
            this.RaisePropertyChanged("ErhaltenerBetrag");
        }

        #region Bindings

        public ICommand OKCommand { get; set; }

        public DateTime Datum { get { return data.Zahldatum; } }
        public Double Betrag { get { return data.Betrag; } }
        public Waehrungen Waehrung { get { return data.Waehrung; } }
        public Double Umrechnungskurs { get { return umrechnungskurs; } }

        public Double ErmittelterBetrag { get { return new DividendenBerechnungen().BetragUmgerechnet(data.Betrag, umrechnungskurs, false, DividendenRundungTypes.Normal); } }
        public Double ErhaltenerBetrag { get { return new DividendenBerechnungen().BetragUmgerechnet(data.Betrag, umrechnungskurs, true, data.RundungArt); } }

        public IEnumerable<DividendenRundungTypes> RundungTypes
        {
            get
            {
                return Enum.GetValues(typeof(DividendenRundungTypes)).Cast<DividendenRundungTypes>();
            }
        }
        public DividendenRundungTypes RundungTyp
        {
            get { return data.RundungArt; }
            set
            {
                if ((this.data.RundungArt != value))
                {
                    this.data.RundungArt = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged("ErhaltenerBetrag");
                }
            }
        }

        #endregion
    }
}