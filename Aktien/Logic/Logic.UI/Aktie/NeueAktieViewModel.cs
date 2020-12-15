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
        public NeueAktieViewModel()
        {
            Title = "Neue Aktie";
            
        }

        public string Title { get; private set; }




    }
}
