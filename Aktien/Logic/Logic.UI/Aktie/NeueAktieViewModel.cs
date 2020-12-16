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

namespace Logic.UI.Aktie
{
    public class NeueAktieViewModel : ViewModelBase
    {
        private string name;
        public NeueAktieViewModel()
        {
            Title = "Neue Aktie";
            name = "";
            SaveNeueAktieCommand = new RelayCommand(() => ExecuteSaveNeueAktieCommand());
        }

        private void ExecuteSaveNeueAktieCommand()
        {
            
        }

        public string Title { get; private set; }

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
