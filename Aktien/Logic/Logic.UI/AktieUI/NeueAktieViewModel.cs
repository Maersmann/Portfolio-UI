using Data;
using Data.API;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Logic.Messages.Aktie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Logic.UI.AktieUI
{
    public class NeueAktieViewModel : ViewModelBasis
    {
        private string name;

        private string isin;

        private int anzahl;
        public NeueAktieViewModel():base()
        {
            Title = "Neue Aktie";
            name = "";
            SaveNeueAktieCommand = new RelayCommand(() => ExecuteSaveNeueAktieCommand());
        }

        private void ExecuteSaveNeueAktieCommand()
        {
            AktieAPI api = new AktieAPI();
            api.Speichern(new Aktie() { Anzahl = anzahl, ISIN = isin, Name = name });
        }

      

        public string ISIN
        {
            get { return this.isin; }
            set
            {

                if (!string.Equals(this.isin, value))
                {
                    this.isin = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public int Anzahl
        {
            get { return this.anzahl; }
            set
            {

                if (!string.Equals(this.anzahl, value))
                {
                    this.anzahl = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public string Name{
            get { return this.name; }
            set
            {
                
                if (!string.Equals(this.name, value))
                {
                    this.name = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public ICommand SaveNeueAktieCommand { get; private set; }


    }
}
