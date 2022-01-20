using Aktien.Logic.Core.Validierung.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
using Data.Model.AuswertungModels.SteueModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace Logic.UI.AuswertungViewModels.SteuerViewModels
{
    public class SteuerMonatgesamtbetragAuswertungViewModel : ViewModelAuswertung<SteuerMonatgesamtbetragAuswertungModel>
    {
        private int jahrvon;
        private int jahrbis;
        private int monat;
        public SteuerMonatgesamtbetragAuswertungViewModel()
        {
            Title = "Auswertung Steuern im Monat";
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
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/steuern/Gesamtbetrag/Monat?monat={monat}&jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<SteuerMonatgesamtbetragAuswertungModel>>();
                if (ItemList.Count() > 0)
                {
                    SelectedItem = ItemList.ElementAt(0);
                }
                RaisePropertyChanged(nameof(ItemList));
            }
            RequestIsWorking = false;
        }


        #region Bindings
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


        public override SteuerMonatgesamtbetragAuswertungModel SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                base.SelectedItem = value;
                RaisePropertyChanged(nameof(DetailItemList));
            }
        }

        public IList<SteuerArtMonatgesamtbetragAuswertungModel> DetailItemList => SelectedItem != null ? SelectedItem.Arten : new List<SteuerArtMonatgesamtbetragAuswertungModel>();
        #endregion

        #region Validate
        private bool ValidatZahl(int? zahl, string fieldname)
        {
            BaseValidierung Validierung = new BaseValidierung();

            bool isValid = Validierung.ValidateAnzahl(zahl, out ICollection<string> validationErrors);

            AddValidateInfo(isValid, fieldname, validationErrors);
            return isValid;
        }
        #endregion
    }
}
