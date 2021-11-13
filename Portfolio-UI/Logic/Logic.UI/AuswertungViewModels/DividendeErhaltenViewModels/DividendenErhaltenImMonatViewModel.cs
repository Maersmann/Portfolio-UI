using Aktien.Logic.Core.Validierung.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.AuswertungModels;
using Data.Model.AuswertungModels.DividendeModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.AuswertungViewModels.DividendeErhaltenViewModels
{
    public class DividendenErhaltenImMonatViewModel : ViewModelAuswertung<DividendenErhaltenImMonatModel>
    {
        private int jahrvon;
        private int jahrbis;
        private int monat;
        public DividendenErhaltenImMonatViewModel()
        {
            Title = "Auswertung Dividende Erhalten im Monat";
            jahrvon = DateTime.Now.Year;
            jahrbis = DateTime.Now.Year;
            monat = DateTime.Now.Month;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividendenErhalten/Monat?monat={monat}&jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<DividendenErhaltenImMonatModel>>();
                if (ItemList.Count() > 0)
                {
                    SelectedItem = ItemList.ElementAt(0);
                }
                RaisePropertyChanged(nameof(ItemList));
                RaisePropertyChanged(nameof(DetailTitle));
            }
            RequestIsWorking = false;
        }


        #region Bindings
        public string DetailTitle => "Auswertung der erhaltenen Dividenden im Monat " + monat;
        public ICommand LoadDataCommand { get; set; }
        public int? JahrVon
        {
            get => jahrvon;
            set
            {
                ValidatZahl(value, nameof(JahrVon));
                RaisePropertyChanged();
                ((DelegateCommand)LoadDataCommand).RaiseCanExecuteChanged();
                jahrvon = value.GetValueOrDefault(0);
            }
        }
        public int? JahrBis
        {
            get => jahrbis;
            set
            {
                ValidatZahl(value, nameof(JahrBis));
                RaisePropertyChanged();
                ((DelegateCommand)LoadDataCommand).RaiseCanExecuteChanged();
                jahrbis = value.GetValueOrDefault(0);
            }
        }
        public int? Monat
        {
            get => monat;
            set
            {
                ValidatZahl(value, nameof(Monat));
                RaisePropertyChanged();
                ((DelegateCommand)LoadDataCommand).RaiseCanExecuteChanged();
                monat = value.GetValueOrDefault(0);
            }
        }

        public override DividendenErhaltenImMonatModel SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                base.SelectedItem = value;
                RaisePropertyChanged(nameof(DetailItemList));
            }
        }

        public IList<DividendenErhaltenImMonatDividendeModel> DetailItemList => SelectedItem != null ? SelectedItem.Dividenden : new List<DividendenErhaltenImMonatDividendeModel>();
        #endregion

        #region Validate
        private bool ValidatZahl(int? zahl, string fieldname)
        {
            var Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateAnzahl(zahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }
        #endregion
    }
}
