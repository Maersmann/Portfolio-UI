using Aktien.Logic.Core;
using Aktien.Logic.UI.BaseViewModels;
using Data.Model.AuswertungModels;
using LiveCharts;
using LiveCharts.Wpf;
using Logic.UI.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Windows.Media;

namespace Logic.UI.AuswertungViewModels
{
    public class DividendeWertpapierAuswertungViewModel : ViewModelAuswertung<DividendeWertpapierAuswertungModel>
    {

        public DividendeWertpapierAuswertungViewModel()
        {
            Title = "Auswertung Dividende je Wertpapier";
            LoadData();
            Formatter = value => value.ToString("0.## €");
        }


        public async void LoadData()
        {
            if (GlobalVariables.ServerIsOnline)
            {
                HttpResponseMessage resp = await Client.GetAsync(GlobalVariables.BackendServer_URL + "/api/auswertung/dividenden/Wertpapiere");
                if (resp.IsSuccessStatusCode)
                    ItemList = await resp.Content.ReadAsAsync<ObservableCollection<DividendeWertpapierAuswertungModel>>();

                ChartValues<double> values = new ChartValues<double>();

                static string labelPoint(ChartPoint chartPoint) =>
                    string.Format("{0:N2}€ ({1:P})", chartPoint.Y, chartPoint.Participation);

                SeriesCollection = new SeriesCollection();

                ItemList.ToList().ForEach(a =>
                {
                    SeriesCollection.Add(new PieSeries { Values = new ChartValues<double> { a.Betrag } , Title = a.Bezeichnung, DataLabels = true, LabelPoint = labelPoint });
                });
               

                RaisePropertyChanged(nameof(SeriesCollection));
                RaisePropertyChanged(nameof(Formatter));
            }
            RaisePropertyChanged("ItemList");
        }
    }
}
