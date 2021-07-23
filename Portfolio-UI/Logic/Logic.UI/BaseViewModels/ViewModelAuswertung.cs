using Aktien.Logic.UI.BaseViewModels;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.UI.BaseViewModels
{
    public class ViewModelAuswertung<T> : ViewModelValidate
    {
        
        public ViewModelAuswertung()
        {
            SeriesCollection = new SeriesCollection();
            ItemList = new List<T>();
            Labels = new[] { "" };
            Formatter = value => value.ToString("F0");
        }

        public T Data { get; set; }
        public IList<T> ItemList;
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }
        public string[] Labels { get; set; }
    }
}
