﻿using Aktien.Logic.Core.Validierung.Base;
using Base.Logic.Core;
using Base.Logic.ViewModels;
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
    public class DividendenErhaltenImJahrViewModel : ViewModelAuswertung<DividendenErhaltenImJahrModel>
    {
        private int jahrvon;
        private int jahrbis;
        public DividendenErhaltenImJahrViewModel()
        {
            Title = "Auswertung Dividende Erhalten im Jahr";
            jahrvon = GlobalUserVariables.JahrVon;
            jahrbis = DateTime.Now.Year;
            LoadDataCommand = new DelegateCommand(ExcecuteLoadDataCommand, CanExcecuteLoadDataCommand);
        }

        private bool CanExcecuteLoadDataCommand()
        {
            return ValidationErrors.Count == 0;
        }

        private async void ExcecuteLoadDataCommand()
        {
            RequestIsWorking = true;
            HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + $"/api/auswertung/dividendenErhalten/Jahr?jahrVon={jahrvon}&jahrBis={jahrbis}");
            if (resp.IsSuccessStatusCode)
            {
                ItemList = await resp.Content.ReadAsAsync<List<DividendenErhaltenImJahrModel>>();
                if (ItemList.Count() > 0)
                {
                    SelectedItem = ItemList.ElementAt(0);
                }
                OnPropertyChanged(nameof(ItemList));
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
                OnPropertyChanged();
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
                OnPropertyChanged();
                ((DelegateCommand)LoadDataCommand).RaiseCanExecuteChanged();
                jahrbis = value.GetValueOrDefault(0);
            }
        }


        public override DividendenErhaltenImJahrModel SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                base.SelectedItem = value;
                OnPropertyChanged(nameof(DetailItemList));
            }
        }

        public IList<DividendenErhaltenImJahrDividendeModel> DetailItemList => SelectedItem != null ? SelectedItem.Dividenden : new List<DividendenErhaltenImJahrDividendeModel>();
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
