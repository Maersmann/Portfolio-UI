using Aktien.Data.Types;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core;
using Aktien.Logic.Core.Validierung.Base;
using Aktien.Logic.Messages.Base;
using Base.Logic.ViewModels;
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
using Base.Logic.Core;
using Base.Logic.Messages;
using Base.Logic.Types;
using Base.Logic.Wrapper;

namespace Logic.UI.SteuerViewModels
{
    public class SteuerStammdatenViewModel : ViewModelOfflineStammdaten<SteuerModel, StammdatenTypes>, IViewModelOfflineStammdaten<SteuerModel>
    {
        private IList<SteuerartModel> steuerarts;
        private string betrag;

        public SteuerStammdatenViewModel()
        {
            steuerarts = new List<SteuerartModel>();
            Title = "Informationen Steuer";
        }

        protected override void ExecuteSaveCommand()
        {
            Gespeichert = true;
            newData = Data.DeepCopy();
            Messenger.Default.Send(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
        }

        protected override StammdatenTypes GetStammdatenTyp() => StammdatenTypes.steuer;


        public void ZeigeStammdatenAn(SteuerModel data)
        {
            Data = data;
            RequestIsWorking = true;
            Waehrung = Data.Waehrung;
            Betrag = Data.Betrag.ToString();
            Optimierung = Data.Optimierung;
            steuerarts.Add(Data.Steuerart);
            Steuerart = Data.Steuerart;    
            RaisePropertyChanged(nameof(Steuerarts));
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            RequestIsWorking = false;
            state = State.Bearbeiten;
        }

        #region Bindings
        public string Betrag
        {
            get => betrag;
            set
            {
                if (!double.TryParse(value, out double Betrag))
                {
                    ValidateBetrag(0);
                    betrag = "";
                    Data.Betrag = 0;
                    RaisePropertyChanged();
                    return;
                }
                betrag = value;
                if (RequestIsWorking || !double.Equals(Data.Betrag, value))
                {
                    ValidateBetrag(Betrag);
                    Data.Betrag = Betrag;
                    RaisePropertyChanged();
                }
            }
        }
        public bool Optimierung
        {
            get => Data.Optimierung;
            set
            {
                if (RequestIsWorking || !bool.Equals(Data.Optimierung, value))
                {
                    Data.Optimierung = value;
                    RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<SteuerartModel> Steuerarts => steuerarts;
        public SteuerartModel Steuerart
        {
            get => Data.Steuerart;
            set
            {
                if (RequestIsWorking || (Data.Steuerart != value))
                {
                    Data.Steuerart = value;
                    RaisePropertyChanged();

                }
            }
        }

        public Waehrungen Waehrung
        {
            get => Data.Waehrung;
            set
            {
                if (RequestIsWorking || (Data.Waehrung != value))
                {
                    Data.Waehrung = value;
                    RaisePropertyChanged();
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
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            return isValid;
        }

        #endregion


        public override void Cleanup()
        {
            state = State.Neu;
            Data = new SteuerModel { Steuerart = new SteuerartModel() };
            Betrag = "";
        }

        public async void LoadSteuerArts(IList<SteuerartModel> vorhandeneSteuerarts)
        {
            if (GlobalVariables.ServerIsOnline)
            {
                RequestIsWorking = true;
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/Steuerarten");

                if (resp.IsSuccessStatusCode)
                {
                    PagedResponse<ObservableCollection<SteuerartModel>> SteuerartResponse = await resp.Content.ReadAsAsync<PagedResponse<ObservableCollection<SteuerartModel>>>();
                    steuerarts = SteuerartResponse.Data;
                }
                    
                else
                    SendExceptionMessage("Fehler beim Laden der Steuerarten");
                RequestIsWorking = false;
            }

            vorhandeneSteuerarts.ToList().ForEach(steuerart =>
            {
                _ = steuerarts.Remove(steuerarts.First(s => s.ID.Equals(steuerart.ID)));
            });
            

            RaisePropertyChanged(nameof(Steuerarts));
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (steuerarts.Count > 0)
            {
                Steuerart = Steuerarts.First();
            }
        }

        protected override bool CanExecuteSaveCommand()
        {
            return base.CanExecuteSaveCommand() && steuerarts.Count > 0;
        }       

    }
}
