using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Core.DividendeLogic.Classes;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeProStueckAnpassenViewModel : ViewModelStammdaten<Dividende>
    {
        private double umrechnungskurs;

        public DividendeProStueckAnpassenViewModel()
        {
            Title = "Dividende pro Stück";
            data = new Dividende();
            RundungTyp = DividendenRundungTypes.Normal;
            umrechnungskurs = 0;
            OKCommand = new RelayCommand(() => ExecuteOKCommand());
        }

        private void ExecuteOKCommand()
        {
            new DividendeAPI().Speichern(data);
            Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende aktualisiert." }, "DividendeProStueckAnpassen");
        }

        public void LoadData(int dividendeErhaltenID, double umrechungskurs)
        {
            data = new DividendeAPI().Lade(dividendeErhaltenID);
            umrechnungskurs = umrechungskurs;
            this.RaisePropertyChanged("Datum");
            this.RaisePropertyChanged("Betrag");
            this.RaisePropertyChanged("Waehrung");
            this.RaisePropertyChanged("Umrechnungskurs");
            this.RaisePropertyChanged("ErmittelterBetrag");
            this.RaisePropertyChanged("ErhaltenerBetrag");
        }

        #region Bindings

        public ICommand OKCommand { get; set; }

        public DateTime Datum { get { return data.Zahldatum; } }
        public Double Betrag { get { return data.Betrag; } }
        public Waehrungen Waehrung { get { return data.Waehrung; } }
        public Double Umrechnungskurs { get { return umrechnungskurs; } }

        public Double ErmittelterBetrag { get { return new DividendenBerechnungen().BetragUmgerechnet(data.Betrag, umrechnungskurs, false, DividendenRundungTypes.Normal); } }
        public Double ErhaltenerBetrag { get { return new DividendenBerechnungen().BetragUmgerechnet(data.Betrag, umrechnungskurs, true, data.RundungArt); } }

        public IEnumerable<DividendenRundungTypes> RundungTypes
        {
            get
            {
                return Enum.GetValues(typeof(DividendenRundungTypes)).Cast<DividendenRundungTypes>();
            }
        }
        public DividendenRundungTypes RundungTyp
        {
            get { return data.RundungArt; }
            set
            {
                if ((this.data.RundungArt != value))
                {
                    this.data.RundungArt = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged("ErhaltenerBetrag");
                }
            }
        }

        #endregion
    }
}
