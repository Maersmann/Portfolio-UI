using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using Aktien.Logic.UI.InterfaceViewModels;
using Data.Model.SteuerModels;
using Data.Types.SteuerTypes;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.SteuerViewModels
{
    public class SteuerStammdatenViewModel : ViewModelStammdaten<SteuerModel>, IViewModelStammdaten
    {
        private int? steuergruppeid;
        private SteuerHerkunftTyp herkunfttyp;
        private IList<SteuerartModel> steuerarts;
        private bool istVerknuepfungGespeichert;

        public SteuerStammdatenViewModel()
        {
            steuerarts = new List<SteuerartModel>();
            Cleanup();
            Title = "Informationen Steuer";
        }

        protected async override void ExecuteSaveCommand()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.PostAsJsonAsync($"https://localhost:5001/api/Steuern?typ={this.herkunfttyp}&gruppeid={this.steuergruppeid}&istVerknuepfungGespeichert={istVerknuepfungGespeichert}", data);


                if (resp.IsSuccessStatusCode)
                {
                    Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());                
                    this.steuergruppeid = await resp.Content.ReadAsAsync<int>();
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage { ID = this.steuergruppeid }, GetStammdatenTyp()); 
                    Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), StammdatenTypes.steuergruppe);
                }
                else
                {
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
                    return;
                }
            }
        }

        public void setGruppeInfos(int? steuergruppeID, SteuerHerkunftTyp typ, object neueSteuergruppe)
        {
            throw new NotImplementedException();
        }

        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.steuer;
        public void setGruppeInfos(int? id, SteuerHerkunftTyp typ, bool istVerknuepfungGespeichert)
        {
            this.steuergruppeid = id;
            this.herkunfttyp = typ;
            this.istVerknuepfungGespeichert = istVerknuepfungGespeichert;
            LoadSteuerArts();
        }

        public async void ZeigeStammdatenAn(int id)
        {
            LoadAktie = true;
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync("https://localhost:5001/api/Steuern/" + id.ToString());
                if (resp.IsSuccessStatusCode)
                    data = await resp.Content.ReadAsAsync<SteuerModel>();
            }
            Waehrung = data.Waehrung;
            Betrag = data.Betrag;
            Optimierung = data.Optimierung;
            Steuerart = data.Steuerart;
            steuerarts.Add(data.Steuerart);
            this.RaisePropertyChanged(nameof(Steuerarts));
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            LoadAktie = false;
            state = State.Bearbeiten;
        }

        #region Bindings
        public double? Betrag
        {
            get { return this.data.Betrag; }
            set
            {
                if (LoadAktie || !double.Equals(this.data.Betrag, value))
                {
                    ValidateBetrag(value);
                    this.data.Betrag = value.GetValueOrDefault(0);
                    this.RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public bool Optimierung
        {
            get { return this.data.Optimierung; }
            set
            {

                if (LoadAktie || !bool.Equals(this.data.Optimierung, value))
                {
                    this.data.Optimierung = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<SteuerartModel> Steuerarts => steuerarts;
        public SteuerartModel Steuerart
        {
            get { return data.Steuerart; }
            set
            {
                if (LoadAktie || (this.data.Steuerart != value))
                {
                    this.data.Steuerart = value;
                    this.RaisePropertyChanged();

                }
            }
        }

        public Waehrungen Waehrung
        {
            get { return data.Waehrung; }
            set
            {
                if (LoadAktie || (this.data.Waehrung != value))
                {
                    this.data.Waehrung = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public static IEnumerable<Waehrungen> Waehrungen => Enum.GetValues(typeof(Waehrungen)).Cast<Waehrungen>();

        #endregion

        #region Validate
        private bool ValidateBetrag(double? betrag)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateBetrag(betrag, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, "Betrag", validationErrors);
            return isValid;
        }

        #endregion


        public override void Cleanup()
        {
            istVerknuepfungGespeichert = false;
            state = State.Neu;
            data = new SteuerModel { Steuerart = new SteuerartModel() };
            Betrag = null;
        }

        private async void LoadSteuerArts()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp;
                if (steuergruppeid.HasValue)
                     resp = await Client.GetAsync($"https://localhost:5001/api/Steuern/Gruppe/{steuergruppeid.Value}/FreieSteuerArten");
                else
                    resp = await Client.GetAsync("https://localhost:5001/api/Steuerarten");
                
                if (resp.IsSuccessStatusCode)
                    steuerarts = await resp.Content.ReadAsAsync<ObservableCollection<SteuerartModel>>();
                else
                    SendExceptionMessage(await resp.Content.ReadAsStringAsync());
            }

            this.RaisePropertyChanged(nameof(Steuerarts));
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (steuerarts.Count > 0)
                Steuerart = Steuerarts.First();
        }

        protected override bool CanExecuteSaveCommand()
        {
            return base.CanExecuteSaveCommand() && steuerarts.Count > 0;
        }
    }
}
